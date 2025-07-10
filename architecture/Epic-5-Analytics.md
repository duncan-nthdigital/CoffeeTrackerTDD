# Epic 5: Daily Summary & Analytics

**Priority:** P1 (High)  
**Phase:** 1 - Anonymous User MVP  
**Dependencies:** Epic 2 (Coffee Logging API)  
**Estimated Time:** 2-3 days  

---

## 📊 Epic Overview

Implement daily coffee consumption analytics and summary features for anonymous users. This provides immediate value by showing users insights about their daily coffee habits, caffeine intake, and consumption patterns.

## 🎯 Success Criteria

- [ ] Users can view daily coffee consumption summary
- [ ] Daily caffeine intake calculation is accurate
- [ ] Basic consumption analytics are displayed
- [ ] Charts/visualizations show consumption patterns
- [ ] All analytics work without user authentication

---

## 📋 Tasks

### Task 5.1: Daily Consumption Analytics API
**Prompt for Copilot:**
```
Using TDD approach, create API endpoints for coffee consumption analytics:

1. Create GET /api/analytics/daily endpoint that returns:
   - Total cups consumed today
   - Total caffeine consumed (mg)
   - Breakdown by coffee type
   - Consumption times throughout the day
   - Average cup size

2. Create GET /api/analytics/weekly endpoint that returns:
   - Daily consumption for last 7 days
   - Weekly average
   - Peak consumption day
   - Caffeine trends

3. Create AnalyticsController with proper error handling
4. Add comprehensive unit tests for all analytics calculations
5. Ensure all calculations are accurate and handle edge cases (no data, partial days)

Use the existing CoffeeEntry model and DbContext. All analytics should be calculated from stored coffee entries.
```

**Files to modify:**
- `src/CoffeeTracker.Api/Controllers/AnalyticsController.cs` (new)
- `src/CoffeeTracker.Api/Models/Analytics/` (new directory)
- `test/CoffeeTracker.Api.Tests/Controllers/AnalyticsControllerTests.cs` (new)

**Acceptance Criteria:**
- [ ] Daily analytics endpoint returns accurate data
- [ ] Weekly analytics endpoint shows trends
- [ ] All calculations are unit tested
- [ ] Error handling for edge cases implemented
- [ ] API documentation is complete

---

### Task 5.2: Caffeine Intake Tracking
**Prompt for Copilot:**
```
Using TDD approach, enhance caffeine tracking capabilities:

1. Create CaffeineCalculator service with methods:
   - CalculateDailyCaffeine(date) - total mg for a specific date
   - CalculateHourlyCaffeine(date) - hourly breakdown
   - GetCaffeineLevel(currentTime) - estimated current caffeine level
   - IsOverRecommendedLimit(date) - check against 400mg FDA recommendation

2. Implement caffeine metabolism calculation:
   - Half-life of caffeine ~5-6 hours
   - Calculate estimated current caffeine level in system
   - Provide warnings when approaching daily limit

3. Add caffeine data to coffee types:
   - Espresso: 64mg per shot
   - Drip coffee: 95mg per 8oz
   - Cold brew: 100mg per 8oz
   - Latte: 64mg (1 shot)
   - Cappuccino: 64mg (1 shot)

4. Create comprehensive tests for all caffeine calculations
```

**Files to modify:**
- `src/CoffeeTracker.Api/Services/CaffeineCalculator.cs` (new)
- `src/CoffeeTracker.Api/Data/SeedData.cs` (update with caffeine data)
- `test/CoffeeTracker.Api.Tests/Services/CaffeineCalculatorTests.cs` (new)

**Acceptance Criteria:**
- [ ] Accurate caffeine content for all coffee types
- [ ] Real-time caffeine level calculation
- [ ] Daily limit warnings implemented
- [ ] Metabolism calculation is scientifically based
- [ ] All calculations are thoroughly tested

---

### Task 5.3: Coffee Consumption Charts Data
**Prompt for Copilot:**
```
Using TDD approach, create API endpoints that provide data for charts and visualizations:

1. Create GET /api/analytics/consumption-by-hour endpoint:
   - Returns coffee consumption grouped by hour of day
   - Includes data for last 7 days
   - Format: { hour: 8, count: 3, avgCaffeine: 192 }

2. Create GET /api/analytics/coffee-type-breakdown endpoint:
   - Returns consumption breakdown by coffee type
   - Includes percentages and counts
   - Format: { type: "Latte", count: 15, percentage: 35.7 }

3. Create GET /api/analytics/consumption-trend endpoint:
   - Returns daily consumption for date range
   - Includes moving averages
   - Format: { date: "2025-01-15", cups: 4, caffeine: 380, movingAvg: 3.8 }

4. Add filtering capabilities:
   - Date range filtering
   - Coffee type filtering
   - Location filtering (coffee shop)

5. Create comprehensive tests for all chart data endpoints
```

**Files to modify:**
- `src/CoffeeTracker.Api/Controllers/AnalyticsController.cs` (update)
- `src/CoffeeTracker.Api/Models/Analytics/ChartData.cs` (new)
- `test/CoffeeTracker.Api.Tests/Controllers/AnalyticsControllerTests.cs` (update)

**Acceptance Criteria:**
- [ ] Chart data endpoints return properly formatted data
- [ ] Filtering options work correctly
- [ ] Moving averages are calculated accurately
- [ ] Data is optimized for common chart libraries
- [ ] All endpoints are thoroughly tested

---

### Task 5.4: Usage Pattern Insights
**Prompt for Copilot:**
```
Using TDD approach, create intelligent insights about user coffee consumption patterns:

1. Create InsightsService with methods:
   - GetPersonalizedInsights(userId) - personalized observations
   - GetConsumptionPatterns() - identify usage patterns
   - GetHealthRecommendations() - caffeine-based health tips
   - GetOptimalTimingRecommendations() - suggest best coffee times

2. Implement pattern detection:
   - Peak consumption hours
   - Weekly patterns (weekday vs weekend)
   - Caffeine sensitivity patterns
   - Most frequent coffee types
   - Favorite coffee shops

3. Create insight generation:
   - "You typically drink 3 cups on weekdays"
   - "Your peak coffee time is 9-10am"
   - "You prefer lattes at Starbucks"
   - "You consume 65% of daily caffeine before noon"

4. Add insight caching and refresh logic
5. Create comprehensive tests for all insight algorithms
```

**Files to modify:**
- `src/CoffeeTracker.Api/Services/InsightsService.cs` (new)
- `src/CoffeeTracker.Api/Models/Analytics/Insight.cs` (new)
- `src/CoffeeTracker.Api/Controllers/AnalyticsController.cs` (update)
- `test/CoffeeTracker.Api.Tests/Services/InsightsServiceTests.cs` (new)

**Acceptance Criteria:**
- [ ] Meaningful insights are generated from consumption data
- [ ] Pattern detection algorithms work correctly
- [ ] Insights are personalized and relevant
- [ ] Caching improves performance
- [ ] All insight logic is unit tested

---

### Task 5.5: Analytics Integration Testing
**Prompt for Copilot:**
```
Create comprehensive integration tests for the analytics system:

1. Create end-to-end analytics test scenarios:
   - User logs multiple coffees over several days
   - Verify daily summary calculations
   - Verify weekly trend calculations
   - Test chart data accuracy
   - Validate insight generation

2. Create performance tests:
   - Test with large datasets (1000+ entries)
   - Verify response times under load
   - Test concurrent analytics requests
   - Memory usage validation

3. Create edge case tests:
   - Analytics with no data
   - Single day with multiple entries
   - Cross-timezone considerations
   - Data boundary conditions

4. Add API integration tests using TestServer
```

**Files to modify:**
- `test/CoffeeTracker.Api.Tests/Integration/AnalyticsIntegrationTests.cs` (new)
- `test/CoffeeTracker.Api.Tests/Performance/AnalyticsPerformanceTests.cs` (new)

**Acceptance Criteria:**
- [ ] All analytics calculations verified end-to-end
- [ ] Performance meets requirements (< 500ms response)
- [ ] Edge cases handled gracefully
- [ ] Integration tests cover realistic scenarios
- [ ] Performance tests pass under load

---

## 🔍 Testing Strategy

### Unit Tests
- All calculation methods must have unit tests
- Edge cases (empty data, extreme values) covered
- Mathematical accuracy verified
- Error conditions tested

### Integration Tests
- Full analytics pipeline tested
- Database queries optimized
- API endpoints tested end-to-end
- Performance benchmarks established

### Acceptance Tests
- User scenarios validated
- Business logic verified
- Insight quality assessed
- Chart data accuracy confirmed

---

## 📈 Success Metrics

- Analytics API response time < 500ms
- Calculation accuracy 100%
- Test coverage > 90%
- Zero critical bugs in analytics
- User insights are meaningful and actionable

---

## 🚀 Deployment Notes

### API Changes
- New analytics endpoints will be added
- No breaking changes to existing endpoints
- Database queries optimized for analytics

### Performance Considerations
- Analytics calculations cached when possible
- Database indexes added for common queries
- Response data minimized for mobile usage

### Monitoring
- Analytics endpoint usage tracked
- Calculation performance monitored
- User engagement with insights measured
