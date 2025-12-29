# Common Formula Examples

## Mathematical

### Sum
```
=SUM(A1:A10)          // Sum range
=SUM(100, 200, 300)   // Sum values
=SUM(A1:A5, C1:C5)    // Sum multiple ranges
```

### Average
```
=AVERAGE(B1:B20)      // Average of range
=AVERAGEIF(A:A, ">100", B:B)  // Conditional average
```

### Count
```
=COUNT(C1:C100)       // Count numbers
=COUNTA(A:A)          // Count non-empty cells
=COUNTIF(D:D, "Yes")  // Count matching values
```

## Logical

### IF
```
=IF(A1>100, "High", "Low")
=IF(B1="", "Empty", B1)
=IF(AND(A1>0, A1<100), "Valid", "Invalid")
```

### Nested IF
```
=IF(A1>=90, "A", IF(A1>=80, "B", IF(A1>=70, "C", "F")))
```

## Text

### Concatenate
```
=CONCATENATE(A1, " ", B1)
=A1&" "&B1                    // Alternative syntax
```

### Text Functions
```
=UPPER(A1)                     // Uppercase
=LOWER(A1)                     // Lowercase
=LEN(A1)                       // Length
=LEFT(A1, 5)                   // First 5 characters
=RIGHT(A1, 3)                  // Last 3 characters
```

## Lookup

### VLOOKUP
```
=VLOOKUP(A1, B1:D10, 2, FALSE)
// Lookup A1 in first column of B1:D10
// Return value from 2nd column
// Exact match
```

### INDEX MATCH
```
=INDEX(C1:C10, MATCH(A1, B1:B10, 0))
// More flexible than VLOOKUP
```

## Date & Time

### Current Date/Time
```
=TODAY()              // Current date
=NOW()                // Current date and time
```

### Date Manipulation
```
=YEAR(A1)            // Extract year
=MONTH(A1)           // Extract month
=DAY(A1)             // Extract day
=DATEDIF(A1, B1, "D") // Days between dates
```

## Statistical

### Min/Max
```
=MIN(A1:A100)        // Minimum value
=MAX(A1:A100)        // Maximum value
```

### Standard Deviation
```
=STDEV(A1:A100)      // Standard deviation
```
