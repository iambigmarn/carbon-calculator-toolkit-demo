const express = require('express');
const cors = require('cors');
const app = express();
const PORT = 5001;

// CORS configuration
const corsOptions = {
  origin: ['http://localhost:3000', 'http://127.0.0.1:3000'],
  methods: ['GET', 'POST', 'PUT', 'DELETE', 'OPTIONS'],
  allowedHeaders: ['Content-Type', 'Authorization', 'Accept'],
  credentials: true
};

// Middleware
app.use(cors(corsOptions));
app.use(express.json());

// Handle preflight requests
app.options('*', cors(corsOptions));

// Mock data
const emissionFactors = [
  { id: 1, category: 'Transportation', subCategory: 'Car', activityType: 'Patient Travel', emissionFactorValue: 0.192, unit: 'km', description: 'Average car emissions per kilometer', source: 'UK Government GHG Conversion Factors' },
  { id: 2, category: 'Transportation', subCategory: 'Train', activityType: 'Patient Travel', emissionFactorValue: 0.041, unit: 'km', description: 'Average train emissions per kilometer', source: 'UK Government GHG Conversion Factors' },
  { id: 3, category: 'Energy', subCategory: 'Electricity', activityType: 'Equipment Usage', emissionFactorValue: 0.233, unit: 'kWh', description: 'UK grid electricity emissions', source: 'UK Government GHG Conversion Factors' },
  { id: 4, category: 'Medical Equipment', subCategory: 'MRI', activityType: 'Imaging', emissionFactorValue: 15.0, unit: 'hour', description: 'MRI scanner energy consumption', source: 'Medical Equipment Energy Database' },
  { id: 5, category: 'Medical Equipment', subCategory: 'CT Scanner', activityType: 'Imaging', emissionFactorValue: 8.0, unit: 'hour', description: 'CT scanner energy consumption', source: 'Medical Equipment Energy Database' },
];

const mitigationStrategies = [
  { id: 1, strategyName: 'Virtual Consultations', category: 'Transportation', description: 'Replace in-person consultations with virtual meetings', implementationSteps: '1. Implement secure video conferencing platform\n2. Train staff on virtual consultation protocols', potentialReductionPercentage: 60.0, costCategory: 'Low', implementationDifficulty: 'Easy', applicableActivities: '["Patient Travel", "Staff Commuting"]' },
  { id: 2, strategyName: 'Renewable Energy Sources', category: 'Energy', description: 'Switch to renewable energy for facility operations', implementationSteps: '1. Conduct energy audit\n2. Install solar panels or wind turbines', potentialReductionPercentage: 80.0, costCategory: 'High', implementationDifficulty: 'Hard', applicableActivities: '["Equipment Usage", "Building Operations"]' },
];

let calculations = [];
let calculationId = 1;

// Routes

// Specific CORS handling for calculator endpoint
app.options('/api/calculator/calculate', cors(corsOptions));

app.get('/api/emissionfactors', (req, res) => {
  const { category, activityType } = req.query;
  let filtered = emissionFactors;
  
  if (category) {
    filtered = filtered.filter(f => f.category === category);
  }
  
  if (activityType) {
    filtered = filtered.filter(f => f.activityType.includes(activityType));
  }
  
  res.json(filtered);
});

app.get('/api/emissionfactors/:id', (req, res) => {
  const factor = emissionFactors.find(f => f.id === parseInt(req.params.id));
  if (!factor) {
    return res.status(404).json({ error: 'Emission factor not found' });
  }
  res.json(factor);
});

app.post('/api/emissionfactors', (req, res) => {
  const newFactor = {
    id: emissionFactors.length + 1,
    ...req.body,
    lastUpdated: new Date().toISOString(),
    isActive: true,
    createdAt: new Date().toISOString(),
    updatedAt: new Date().toISOString()
  };
  emissionFactors.push(newFactor);
  res.status(201).json(newFactor);
});

app.get('/api/mitigationstrategies', (req, res) => {
  const { category, costCategory, difficulty } = req.query;
  let filtered = mitigationStrategies;
  
  if (category) {
    filtered = filtered.filter(s => s.category === category);
  }
  
  if (costCategory) {
    filtered = filtered.filter(s => s.costCategory === costCategory);
  }
  
  if (difficulty) {
    filtered = filtered.filter(s => s.implementationDifficulty === difficulty);
  }
  
  res.json(filtered);
});

app.get('/api/mitigationstrategies/:id', (req, res) => {
  const strategy = mitigationStrategies.find(s => s.id === parseInt(req.params.id));
  if (!strategy) {
    return res.status(404).json({ error: 'Mitigation strategy not found' });
  }
  res.json(strategy);
});

app.post('/api/calculator/calculate', (req, res) => {
  const { trialId, userId, calculationName, activities } = req.body;
  
  if (!activities || activities.length === 0) {
    return res.status(400).json({ error: 'Invalid request. Activities are required.' });
  }
  
  const newCalculationId = `calc-${Date.now()}-${calculationId++}`;
  const breakdown = [];
  let totalEmissions = 0;
  
  // Process each activity
  for (const activity of activities) {
    const factor = emissionFactors.find(f => 
      f.activityType === activity.activityType && f.unit === activity.unit
    );
    
    if (!factor) {
      return res.status(400).json({ 
        error: `No emission factor found for activity type: ${activity.activityType} with unit: ${activity.unit}` 
      });
    }
    
    const calculatedEmissions = activity.quantity * factor.emissionFactorValue;
    totalEmissions += calculatedEmissions;
    
    breakdown.push({
      activityType: activity.activityType,
      quantity: activity.quantity,
      unit: activity.unit,
      emissionFactor: factor.emissionFactorValue,
      calculatedEmissions: calculatedEmissions,
      percentage: 0 // Will be calculated below
    });
  }
  
  // Calculate percentages
  breakdown.forEach(item => {
    item.percentage = totalEmissions > 0 ? (item.calculatedEmissions / totalEmissions) * 100 : 0;
  });
  
  // Identify hotspots
  const hotspots = breakdown.map(item => {
    let severity, recommendation;
    
    if (item.percentage >= 50) {
      severity = 'Critical';
      recommendation = 'Immediate action required. This activity represents over 50% of total emissions.';
    } else if (item.percentage >= 25) {
      severity = 'High';
      recommendation = 'High priority for mitigation. Consider alternative approaches.';
    } else if (item.percentage >= 10) {
      severity = 'Medium';
      recommendation = 'Moderate priority. Review for optimization opportunities.';
    } else {
      severity = 'Low';
      recommendation = 'Low priority. Monitor for future optimization.';
    }
    
    return {
      activityType: item.activityType,
      emissions: item.calculatedEmissions,
      percentage: item.percentage,
      severity,
      recommendation
    };
  }).sort((a, b) => b.percentage - a.percentage);
  
  // Get mitigation strategies for high priority hotspots
  const highPriorityHotspots = hotspots.filter(h => h.severity === 'High' || h.severity === 'Critical');
  const mitigationStrategiesForHotspots = [];
  
  for (const hotspot of highPriorityHotspots) {
    const relevantStrategies = mitigationStrategies.filter(s => 
      s.applicableActivities && s.applicableActivities.includes(hotspot.activityType)
    ).slice(0, 2);
    
    mitigationStrategiesForHotspots.push(...relevantStrategies.map(s => ({
      strategyId: s.id,
      strategyName: s.strategyName,
      category: s.category,
      description: s.description,
      potentialReduction: s.potentialReductionPercentage,
      implementationSteps: s.implementationSteps
    })));
  }
  
  // Remove duplicates
  const uniqueStrategies = mitigationStrategiesForHotspots.filter((strategy, index, self) => 
    index === self.findIndex(s => s.strategyId === strategy.strategyId)
  );
  
  const result = {
    calculationId: newCalculationId,
    trialId,
    userId,
    calculationName: calculationName || `Calculation ${new Date().toLocaleDateString()}`,
    totalEmissions,
    unit: 'kg CO2e',
    calculationDate: new Date().toISOString(),
    status: 'Completed',
    breakdown,
    hotspots,
    mitigationStrategies: uniqueStrategies
  };
  
  // Store calculation
  calculations.unshift(result);
  
  res.json(result);
});

app.get('/api/calculator/history', (req, res) => {
  console.log('📋 Received history request:', req.url);
  const { trialId, userId } = req.query;
  let filtered = calculations;
  
  if (trialId) {
    filtered = filtered.filter(c => c.trialId === trialId);
  }
  
  if (userId) {
    filtered = filtered.filter(c => c.userId === userId);
  }
  
  // Transform the data to match the expected interface for CalculationHistory
  const historyData = filtered.map(calc => ({
    id: calc.calculationId,
    trialId: calc.trialId,
    userId: calc.userId,
    calculationName: calc.calculationName,
    totalEmissions: calc.totalEmissions,
    unit: calc.unit,
    calculationDate: calc.calculationDate,
    status: calc.status,
    activityCount: calc.breakdown ? calc.breakdown.length : 0,
    highestSeverity: calc.hotspots && calc.hotspots.length > 0 
      ? calc.hotspots.reduce((highest, hotspot) => {
          const severityOrder = { 'Critical': 4, 'High': 3, 'Medium': 2, 'Low': 1 };
          return severityOrder[hotspot.severity] > severityOrder[highest] ? hotspot.severity : highest;
        }, 'Low')
      : 'Low'
  }));
  
  res.json(historyData);
});

// New endpoint for Dashboard to get full calculation data
app.get('/api/calculator/dashboard-data', (req, res) => {
  console.log('📊 Received dashboard-data request:', req.url);
  const { trialId, userId } = req.query;
  let filtered = calculations;
  
  if (trialId) {
    filtered = filtered.filter(c => c.trialId === trialId);
  }
  
  if (userId) {
    filtered = filtered.filter(c => c.userId === userId);
  }
  
  // Return the full calculation data for Dashboard
  res.json(filtered);
});

app.get('/api/calculator/:id', (req, res) => {
  const calculation = calculations.find(c => c.calculationId === req.params.id);
  if (!calculation) {
    return res.status(404).json({ error: 'Calculation not found' });
  }
  
  const details = {
    calculation: {
      id: calculation.calculationId,
      trialId: calculation.trialId,
      userId: calculation.userId,
      calculationName: calculation.calculationName,
      totalEmissions: calculation.totalEmissions,
      unit: calculation.unit,
      calculationDate: calculation.calculationDate,
      status: calculation.status
    },
    activities: calculation.breakdown.map(item => ({
      id: `activity-${Math.random()}`,
      activityType: item.activityType,
      quantity: item.quantity,
      unit: item.unit,
      emissionFactor: item.emissionFactor,
      calculatedEmissions: item.calculatedEmissions,
      activityDescription: item.description
    })),
    hotspots: calculation.hotspots.map((hotspot, index) => ({
      id: index + 1,
      activityType: hotspot.activityType,
      emissions: hotspot.emissions,
      percentageOfTotal: hotspot.percentage,
      severity: hotspot.severity,
      recommendation: hotspot.recommendation
    }))
  };
  
  res.json(details);
});

// Health check
app.get('/api/health', (req, res) => {
  res.json({ status: 'OK', timestamp: new Date().toISOString() });
});

app.listen(PORT, () => {
  console.log(`🚀 Mock API server running on http://localhost:${PORT}`);
  console.log(`📚 API Documentation: http://localhost:${PORT}/api/health`);
  console.log(`🌱 Carbon Calculator Toolkit - Mock Backend`);
  console.log(`🔧 CORS enabled for: http://localhost:3000`);
  console.log(`📡 Ready to accept requests from frontend`);
});
