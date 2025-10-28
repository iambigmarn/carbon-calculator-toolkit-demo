-- Sample Data for Carbon Calculator Toolkit
-- Seed data for demonstration purposes

USE CarbonCalculatorToolkit;
GO

-- Insert sample emission factors
INSERT INTO EmissionFactors (Category, SubCategory, ActivityType, EmissionFactor, Unit, Description, Source) VALUES
-- Transportation Factors
('Transportation', 'Car', 'Patient Travel', 0.192, 'km', 'Average car emissions per kilometer', 'UK Government GHG Conversion Factors'),
('Transportation', 'Car', 'Staff Commuting', 0.192, 'km', 'Average car emissions per kilometer', 'UK Government GHG Conversion Factors'),
('Transportation', 'Train', 'Patient Travel', 0.041, 'km', 'Average train emissions per kilometer', 'UK Government GHG Conversion Factors'),
('Transportation', 'Train', 'Staff Commuting', 0.041, 'km', 'Average train emissions per kilometer', 'UK Government GHG Conversion Factors'),
('Transportation', 'Bus', 'Patient Travel', 0.089, 'km', 'Average bus emissions per kilometer', 'UK Government GHG Conversion Factors'),
('Transportation', 'Plane', 'Staff Travel', 0.285, 'km', 'Average domestic flight emissions per kilometer', 'UK Government GHG Conversion Factors'),

-- Energy Factors
('Energy', 'Electricity', 'Equipment Usage', 0.233, 'kWh', 'UK grid electricity emissions', 'UK Government GHG Conversion Factors'),
('Energy', 'Electricity', 'Building Operations', 0.233, 'kWh', 'UK grid electricity emissions', 'UK Government GHG Conversion Factors'),
('Energy', 'Natural Gas', 'Heating', 0.202, 'kWh', 'Natural gas combustion emissions', 'UK Government GHG Conversion Factors'),
('Energy', 'Natural Gas', 'Hot Water', 0.202, 'kWh', 'Natural gas combustion emissions', 'UK Government GHG Conversion Factors'),

-- Medical Equipment Factors
('Medical Equipment', 'MRI', 'Imaging', 15.0, 'hour', 'MRI scanner energy consumption', 'Medical Equipment Energy Database'),
('Medical Equipment', 'CT Scanner', 'Imaging', 8.0, 'hour', 'CT scanner energy consumption', 'Medical Equipment Energy Database'),
('Medical Equipment', 'Ultrasound', 'Imaging', 2.5, 'hour', 'Ultrasound machine energy consumption', 'Medical Equipment Energy Database'),
('Medical Equipment', 'X-Ray', 'Imaging', 4.0, 'hour', 'X-Ray machine energy consumption', 'Medical Equipment Energy Database'),
('Medical Equipment', 'Laboratory Equipment', 'Testing', 3.0, 'hour', 'Average lab equipment energy consumption', 'Laboratory Energy Assessment'),

-- Laboratory Factors
('Laboratory', 'Chemical Disposal', 'Waste Management', 0.5, 'kg', 'Chemical waste disposal emissions', 'Environmental Impact Assessment'),
('Laboratory', 'Sample Storage', 'Cold Storage', 0.8, 'day', 'Refrigerated storage emissions', 'Cold Storage Energy Study'),
('Laboratory', 'Water Usage', 'Lab Operations', 0.0003, 'liter', 'Water treatment and heating emissions', 'Water Treatment GHG Factors'),

-- Data and IT Factors
('IT', 'Data Storage', 'Cloud Computing', 0.0004, 'GB', 'Cloud storage emissions', 'Cloud Provider GHG Reports'),
('IT', 'Data Processing', 'Computing', 0.0002, 'hour', 'Server processing emissions', 'Data Center Energy Study'),
('IT', 'Video Conferencing', 'Remote Meetings', 0.001, 'hour', 'Video call energy consumption', 'Telecommunications GHG Study'),

-- Waste Management Factors
('Waste', 'Medical Waste', 'Disposal', 0.8, 'kg', 'Medical waste incineration emissions', 'Waste Management GHG Factors'),
('Waste', 'Paper Waste', 'Disposal', 0.3, 'kg', 'Paper recycling/disposal emissions', 'Waste Management GHG Factors'),
('Waste', 'Plastic Waste', 'Disposal', 0.4, 'kg', 'Plastic disposal emissions', 'Waste Management GHG Factors');

-- Insert sample trial information
INSERT INTO TrialInformation (TrialId, TrialName, PrincipalInvestigator, Institution, TrialPhase, PatientCount, DurationMonths, Description) VALUES
('TRIAL-001', 'Phase III Oncology Drug Study', 'Dr. Sarah Johnson', 'Imperial College London', 'Phase III', 500, 24, 'Randomized controlled trial for new cancer treatment'),
('TRIAL-002', 'Cardiovascular Prevention Study', 'Dr. Michael Chen', 'University College London', 'Phase II', 200, 18, 'Study on cardiovascular disease prevention strategies'),
('TRIAL-003', 'Diabetes Management Trial', 'Dr. Emily Rodriguez', 'King''s College London', 'Phase II', 150, 12, 'Comparative study of diabetes management approaches'),
('TRIAL-004', 'Pediatric Vaccine Study', 'Dr. James Wilson', 'Great Ormond Street Hospital', 'Phase I', 50, 6, 'Safety and efficacy study for new pediatric vaccine'),
('TRIAL-005', 'Mental Health Intervention', 'Dr. Lisa Thompson', 'South London and Maudsley NHS', 'Phase III', 300, 36, 'Long-term study of mental health intervention effectiveness');

-- Insert sample mitigation strategies
INSERT INTO MitigationStrategies (StrategyName, Category, Description, ImplementationSteps, PotentialReductionPercentage, CostCategory, ImplementationDifficulty, ApplicableActivities) VALUES
('Virtual Consultations', 'Transportation', 'Replace in-person consultations with virtual meetings', '1. Implement secure video conferencing platform
2. Train staff on virtual consultation protocols
3. Update patient communication materials
4. Establish technical support system', 60.0, 'Low', 'Easy', '["Patient Travel", "Staff Commuting"]'),

('Renewable Energy Sources', 'Energy', 'Switch to renewable energy for facility operations', '1. Conduct energy audit
2. Install solar panels or wind turbines
3. Negotiate renewable energy contracts
4. Monitor energy consumption', 80.0, 'High', 'Hard', '["Equipment Usage", "Building Operations"]'),

('Equipment Efficiency Upgrade', 'Medical Equipment', 'Replace old equipment with energy-efficient models', '1. Audit current equipment energy usage
2. Research energy-efficient alternatives
3. Plan phased replacement schedule
4. Train staff on new equipment', 40.0, 'High', 'Medium', '["Imaging", "Testing"]'),

('Digital Documentation', 'Waste', 'Reduce paper usage through digital systems', '1. Implement electronic health records
2. Use digital consent forms
3. Provide tablets for patient questionnaires
4. Train staff on digital workflows', 70.0, 'Medium', 'Medium', '["Paper Waste"]'),

('Carpooling Program', 'Transportation', 'Encourage staff carpooling to reduce commuting emissions', '1. Survey staff commuting patterns
2. Create carpooling matching system
3. Provide incentives for participation
4. Monitor program effectiveness', 25.0, 'Low', 'Easy', '["Staff Commuting"]'),

('Green Laboratory Practices', 'Laboratory', 'Implement sustainable laboratory practices', '1. Optimize equipment usage schedules
2. Implement waste reduction programs
3. Use energy-efficient lab equipment
4. Train staff on green practices', 30.0, 'Medium', 'Medium', '["Chemical Disposal", "Sample Storage", "Lab Operations"]'),

('Cloud Migration', 'IT', 'Move data storage and processing to efficient cloud providers', '1. Assess current IT infrastructure
2. Select green cloud provider
3. Plan migration strategy
4. Implement cloud solutions', 50.0, 'Medium', 'Medium', '["Data Storage", "Data Processing"]'),

('Waste Segregation', 'Waste', 'Improve waste segregation to increase recycling rates', '1. Install proper waste bins
2. Train staff on waste segregation
3. Partner with recycling companies
4. Monitor waste streams', 35.0, 'Low', 'Easy', '["Medical Waste", "Paper Waste", "Plastic Waste"]');

-- Insert sample calculation history
INSERT INTO CalculationHistory (Id, TrialId, UserId, CalculationName, TotalEmissions, Status) VALUES
(NEWID(), 'TRIAL-001', 'user001', 'Baseline Carbon Footprint Assessment', 1250.5, 'Completed'),
(NEWID(), 'TRIAL-002', 'user002', 'Q1 2024 Carbon Assessment', 890.3, 'Completed'),
(NEWID(), 'TRIAL-003', 'user001', 'Pre-Intervention Baseline', 675.8, 'Completed'),
(NEWID(), 'TRIAL-004', 'user003', 'Pilot Study Assessment', 320.2, 'Completed'),
(NEWID(), 'TRIAL-005', 'user002', 'Annual Carbon Review', 2100.7, 'Completed');

-- Insert sample hotspots (linked to first calculation)
DECLARE @CalcId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM CalculationHistory WHERE TrialId = 'TRIAL-001');

INSERT INTO Hotspots (CalculationId, ActivityType, Emissions, PercentageOfTotal, Severity, Recommendation, MitigationStrategyId) VALUES
(@CalcId, 'Patient Travel', 750.3, 60.0, 'High', 'Consider implementing virtual consultations for follow-up visits', 1),
(@CalcId, 'Staff Commuting', 250.1, 20.0, 'Medium', 'Implement carpooling program and encourage public transport use', 5),
(@CalcId, 'Equipment Usage', 125.0, 10.0, 'Low', 'Schedule equipment usage more efficiently', 3),
(@CalcId, 'Building Operations', 125.1, 10.0, 'Low', 'Consider renewable energy sources for facility', 2);

PRINT 'Sample data inserted successfully!';
PRINT 'Emission Factors: 25 records';
PRINT 'Trial Information: 5 records';
PRINT 'Mitigation Strategies: 8 records';
PRINT 'Calculation History: 5 records';
PRINT 'Hotspots: 4 records';
