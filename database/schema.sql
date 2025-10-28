-- Carbon Calculator Toolkit Database Schema
-- MS-SQL Server Database Design for ICR-CTSU Demo

-- Create database
CREATE DATABASE CarbonCalculatorToolkit;
GO

USE CarbonCalculatorToolkit;
GO

-- Emission Factors Table
-- Stores carbon emission factors for various activities
CREATE TABLE EmissionFactors (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Category NVARCHAR(100) NOT NULL, -- e.g., 'Transportation', 'Energy', 'Equipment'
    SubCategory NVARCHAR(100) NOT NULL, -- e.g., 'Car', 'Train', 'Plane'
    ActivityType NVARCHAR(200) NOT NULL, -- e.g., 'Patient Travel', 'Staff Commuting'
    EmissionFactor DECIMAL(10,6) NOT NULL, -- kg CO2e per unit
    Unit NVARCHAR(50) NOT NULL, -- e.g., 'km', 'kWh', 'hour'
    Description NVARCHAR(500),
    Source NVARCHAR(200), -- Reference source for the factor
    LastUpdated DATETIME2 DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE()
);

-- Calculation History Table
-- Tracks all carbon footprint calculations performed
CREATE TABLE CalculationHistory (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    TrialId NVARCHAR(100) NOT NULL,
    UserId NVARCHAR(100),
    CalculationName NVARCHAR(200),
    TotalEmissions DECIMAL(12,4) NOT NULL, -- Total kg CO2e
    Unit NVARCHAR(50) DEFAULT 'kg CO2e',
    CalculationDate DATETIME2 DEFAULT GETDATE(),
    Status NVARCHAR(50) DEFAULT 'Completed', -- Completed, Failed, InProgress
    ErrorMessage NVARCHAR(1000),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE()
);

-- Calculation Activities Table
-- Stores individual activities within each calculation
CREATE TABLE CalculationActivities (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    CalculationId UNIQUEIDENTIFIER NOT NULL,
    ActivityType NVARCHAR(200) NOT NULL,
    Quantity DECIMAL(12,4) NOT NULL,
    Unit NVARCHAR(50) NOT NULL,
    EmissionFactor DECIMAL(10,6) NOT NULL,
    CalculatedEmissions DECIMAL(12,4) NOT NULL,
    ActivityDescription NVARCHAR(500),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (CalculationId) REFERENCES CalculationHistory(Id) ON DELETE CASCADE
);

-- Mitigation Strategies Table
-- Stores carbon reduction strategies and recommendations
CREATE TABLE MitigationStrategies (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StrategyName NVARCHAR(200) NOT NULL,
    Category NVARCHAR(100) NOT NULL, -- e.g., 'Transportation', 'Energy', 'Equipment'
    Description NVARCHAR(1000) NOT NULL,
    ImplementationSteps NVARCHAR(2000),
    PotentialReductionPercentage DECIMAL(5,2), -- Percentage reduction possible
    CostCategory NVARCHAR(50), -- Low, Medium, High
    ImplementationDifficulty NVARCHAR(50), -- Easy, Medium, Hard
    ApplicableActivities NVARCHAR(1000), -- JSON array of applicable activity types
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE()
);

-- Hotspots Table
-- Identifies high-emission activities and areas for improvement
CREATE TABLE Hotspots (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CalculationId UNIQUEIDENTIFIER NOT NULL,
    ActivityType NVARCHAR(200) NOT NULL,
    Emissions DECIMAL(12,4) NOT NULL,
    PercentageOfTotal DECIMAL(5,2) NOT NULL,
    Severity NVARCHAR(50) NOT NULL, -- Low, Medium, High, Critical
    Recommendation NVARCHAR(1000),
    MitigationStrategyId INT,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (CalculationId) REFERENCES CalculationHistory(Id) ON DELETE CASCADE,
    FOREIGN KEY (MitigationStrategyId) REFERENCES MitigationStrategies(Id)
);

-- Trial Information Table
-- Stores basic information about clinical trials
CREATE TABLE TrialInformation (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TrialId NVARCHAR(100) UNIQUE NOT NULL,
    TrialName NVARCHAR(300) NOT NULL,
    PrincipalInvestigator NVARCHAR(200),
    Institution NVARCHAR(300),
    TrialPhase NVARCHAR(50), -- Phase I, II, III, IV
    PatientCount INT,
    DurationMonths INT,
    Description NVARCHAR(1000),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE()
);

-- Create indexes for performance optimization
CREATE INDEX IX_EmissionFactors_Category ON EmissionFactors(Category);
CREATE INDEX IX_EmissionFactors_ActivityType ON EmissionFactors(ActivityType);
CREATE INDEX IX_EmissionFactors_IsActive ON EmissionFactors(IsActive);

CREATE INDEX IX_CalculationHistory_TrialId ON CalculationHistory(TrialId);
CREATE INDEX IX_CalculationHistory_CalculationDate ON CalculationHistory(CalculationDate);
CREATE INDEX IX_CalculationHistory_UserId ON CalculationHistory(UserId);

CREATE INDEX IX_CalculationActivities_CalculationId ON CalculationActivities(CalculationId);
CREATE INDEX IX_CalculationActivities_ActivityType ON CalculationActivities(ActivityType);

CREATE INDEX IX_MitigationStrategies_Category ON MitigationStrategies(Category);
CREATE INDEX IX_MitigationStrategies_IsActive ON MitigationStrategies(IsActive);

CREATE INDEX IX_Hotspots_CalculationId ON Hotspots(CalculationId);
CREATE INDEX IX_Hotspots_Severity ON Hotspots(Severity);

CREATE INDEX IX_TrialInformation_TrialId ON TrialInformation(TrialId);
CREATE INDEX IX_TrialInformation_IsActive ON TrialInformation(IsActive);

-- Create views for common queries
CREATE VIEW vw_ActiveEmissionFactors AS
SELECT 
    Id,
    Category,
    SubCategory,
    ActivityType,
    EmissionFactor,
    Unit,
    Description,
    Source,
    LastUpdated
FROM EmissionFactors
WHERE IsActive = 1;

CREATE VIEW vw_CalculationSummary AS
SELECT 
    ch.Id,
    ch.TrialId,
    ti.TrialName,
    ch.CalculationName,
    ch.TotalEmissions,
    ch.CalculationDate,
    ch.Status,
    COUNT(ca.Id) as ActivityCount,
    MAX(h.Severity) as HighestSeverity
FROM CalculationHistory ch
LEFT JOIN TrialInformation ti ON ch.TrialId = ti.TrialId
LEFT JOIN CalculationActivities ca ON ch.Id = ca.CalculationId
LEFT JOIN Hotspots h ON ch.Id = h.CalculationId
GROUP BY ch.Id, ch.TrialId, ti.TrialName, ch.CalculationName, 
         ch.TotalEmissions, ch.CalculationDate, ch.Status;

-- Create stored procedures for common operations
CREATE PROCEDURE sp_CalculateCarbonFootprint
    @TrialId NVARCHAR(100),
    @Activities NVARCHAR(MAX), -- JSON array of activities
    @UserId NVARCHAR(100) = NULL,
    @CalculationName NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CalculationId UNIQUEIDENTIFIER = NEWID();
    DECLARE @TotalEmissions DECIMAL(12,4) = 0;
    
    -- Insert calculation record
    INSERT INTO CalculationHistory (Id, TrialId, UserId, CalculationName, TotalEmissions)
    VALUES (@CalculationId, @TrialId, @UserId, @CalculationName, 0);
    
    -- Process activities (simplified - would need JSON parsing in real implementation)
    -- This is a placeholder for the actual calculation logic
    
    -- Update total emissions
    UPDATE CalculationHistory 
    SET TotalEmissions = @TotalEmissions,
        UpdatedAt = GETDATE()
    WHERE Id = @CalculationId;
    
    SELECT @CalculationId as CalculationId, @TotalEmissions as TotalEmissions;
END;

-- Create function to get emission factor
CREATE FUNCTION fn_GetEmissionFactor(@ActivityType NVARCHAR(200), @Unit NVARCHAR(50))
RETURNS DECIMAL(10,6)
AS
BEGIN
    DECLARE @Factor DECIMAL(10,6);
    
    SELECT @Factor = EmissionFactor
    FROM EmissionFactors
    WHERE ActivityType = @ActivityType 
      AND Unit = @Unit 
      AND IsActive = 1;
    
    RETURN ISNULL(@Factor, 0);
END;

PRINT 'Carbon Calculator Toolkit database schema created successfully!';
PRINT 'Database: CarbonCalculatorToolkit';
PRINT 'Tables created: 6';
PRINT 'Views created: 2';
PRINT 'Stored procedures created: 1';
PRINT 'Functions created: 1';
PRINT 'Indexes created: 12';
