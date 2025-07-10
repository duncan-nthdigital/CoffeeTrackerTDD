# Issue #015: Create Coffee Shop Seed Data

**Epic:** Epic 3 - Coffee Shop Data & Locator API  
**Task:** 3.3 - Create Coffee Shop Seed Data  
**Estimated Time:** 2-3 hours  
**Priority:** Medium  
**Dependencies:** Issue #004 (Coffee Shop Basic Model), Issue #005 (Database Migration)

---

## 📝 Description

Create comprehensive seed data for coffee shops to support development and testing scenarios. This includes creating realistic test data with diverse shop types, names, and locations that will enable proper testing of search functionality, pagination, and location-based features.

## 🎯 Acceptance Criteria

### Data Requirements
- [ ] At least 50 diverse coffee shop entries created
- [ ] Realistic names, addresses, and locations included
- [ ] Mix of chain and independent shops represented
- [ ] Data supports search and filtering test scenarios
- [ ] Seed data runs during database initialization automatically

### Shop Diversity Requirements
- [ ] Chain coffee shops (Starbucks-style, Costa-style, etc.)
- [ ] Independent local coffee shops with creative names
- [ ] Specialty/artisan coffee roasters
- [ ] Café/bistro combination establishments
- [ ] Various naming styles (modern, traditional, quirky)

### Geographic Diversity
- [ ] Urban downtown addresses with numbered streets
- [ ] Suburban residential area addresses
- [ ] Small town main street locations
- [ ] Various address formats and international styles
- [ ] Coordinates included for location-based testing (if geographic support added)

### Data Quality
- [ ] All required fields populated correctly
- [ ] No duplicate shop names in seed data
- [ ] All shops marked as active (IsActive = true)
- [ ] CreatedAt dates spread across past month
- [ ] Addresses formatted consistently

## 🔧 Technical Details

### Implementation Requirements
- Create `CoffeeShopSeeder` class with static seeding method
- Check if data already exists before seeding to avoid duplicates
- Use realistic but fictional addresses to avoid real business conflicts
- Integrate with database initialization process
- Handle seeding errors gracefully with proper logging

### Data Patterns
- Include shops that test alphabetical sorting edge cases
- Names that support partial search matching tests
- Address variations that test location search functionality
- Shop names with special characters and international elements

### Testing Support
- Data should enable comprehensive search functionality testing
- Support pagination testing with various page sizes
- Include edge cases for search algorithms (very short/long names)
- Enable performance testing with realistic dataset size

## 🤖 Copilot Prompt

```csharp
Create comprehensive seed data for coffee shops in a .NET 8 Web API project.

Requirements:
- Create at least 50 diverse coffee shop entries
- Include mix of:
  - Chain coffee shops (Starbucks, Costa, etc.)
  - Independent local coffee shops
  - Specialty/artisan coffee roasters
  - Café/bistro combinations
- Realistic data including:
  - Creative but believable shop names
  - Diverse address formats and locations
  - Mix of urban, suburban, and small town locations
  - Variety in naming styles (modern, traditional, quirky)

Seed data should support testing:
- Search functionality (partial name matches)
- Alphabetical sorting
- Pagination scenarios
- Geographic diversity

Create `CoffeeShopSeeder` class with:
- Static method: `SeedCoffeeShops(CoffeeTrackerDbContext context)`
- Check if data already exists before seeding
- Use realistic but fictional addresses
- Set all shops as IsActive = true
- Set CreatedAt to various dates in the past month

Example shop types to include:
- "The Daily Grind", "Bean There Coffee Co.", "Roasted & Ready"
- "Café Mocha", "Espresso Express", "The Coffee Corner"
- "Java Junction", "Brew & Bite", "Steam & Bean"
- Chain references: "Metro Coffee", "City Roast", "Quick Cup"

Geographic diversity:
- Urban: downtown addresses, numbered streets
- Suburban: residential area addresses
- Small town: main street addresses
- Various formats: "123 Main St", "45 Oak Avenue", "Unit 5, Shopping Plaza"

Create unit tests that verify:
- Seed data loads correctly
- All shops have valid required fields
- No duplicate names in seed data
- Search functionality works with seeded data

Integrate with database initialization:
- Call seeder in DbContext initialization
- Ensure seeding only happens once
- Handle errors gracefully

Place seeder in `Data/Seeders/CoffeeShopSeeder.cs` and update DbContext initialization.
```

## 📁 File Locations

```
src/CoffeeTracker.Api/
├── Data/
│   ├── Seeders/
│   │   └── CoffeeShopSeeder.cs         # Seed data class
│   └── CoffeeTrackerDbContext.cs       # Updated with seeder integration
test/CoffeeTracker.Api.Tests/
├── Data/
│   └── CoffeeShopSeederTests.cs        # Seeder validation tests
```

## ✅ Definition of Done

- [ ] `CoffeeShopSeeder` class created with comprehensive seed data
- [ ] At least 50 realistic coffee shop entries
- [ ] Diverse mix of shop types and naming styles
- [ ] Geographic diversity across urban, suburban, and small town locations
- [ ] Integration with database initialization process
- [ ] Unit tests verify seed data quality and uniqueness
- [ ] All tests passing
- [ ] Seeder handles duplicate prevention correctly
- [ ] Error handling and logging implemented
- [ ] Performance acceptable for development database initialization
- [ ] Documentation updated with seeding process
- [ ] No real business names used to avoid conflicts

## 🔗 Related Issues

- **Depends on:** Issue #004 (Coffee Shop Basic Model)
- **Depends on:** Issue #005 (Database Migration)
- **Blocks:** Issue #014 (Coffee Shop Service Layer)
- **Blocks:** Issue #017 (Coffee Shop Integration Tests)

## 📋 Notes

### Example Shop Names by Category

**Chain-Style Names:**
- Metro Coffee Co.
- City Roast
- Quick Cup Express
- Brew Central
- Coffee & More

**Independent Shops:**
- The Daily Grind
- Bean There Coffee Co.
- Roasted & Ready
- Steam & Bean
- The Coffee Nook

**Artisan/Specialty:**
- Third Wave Roasters
- Single Origin
- Craft Coffee Co.
- The Roastery
- Artisan Brew House

**Café/Bistro Style:**
- Café Mocha
- The Coffee Corner
- Java Junction
- Brew & Bite
- Morning Glory Café

### Address Format Examples
- Urban: "123 5th Avenue, Downtown", "456 Market Street, Suite 101"
- Suburban: "789 Oak Lane", "321 Maple Drive"
- Small Town: "45 Main Street", "12 Town Square"
- Shopping Centers: "Unit 15, Westfield Shopping Center"

### Testing Considerations
- Include names that test case-insensitive search
- Add shops with special characters (apostrophes, hyphens)
- Include very short names (2-3 characters) and longer names
- Ensure geographical spread for location-based testing
- Add shops with similar names to test search ranking
