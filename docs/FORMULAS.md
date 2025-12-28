# Formula Reference

## Mathematical Functions

### SUM

Adds numbers together.

```
=SUM(A1:A10)
=SUM(100, 200, 300)
```

### AVERAGE

Calculates the average.

```
=AVERAGE(B1:B20)
```

### COUNT

Counts numeric values.

```
=COUNT(C1:C50)
```

### MAX / MIN

Returns maximum or minimum value.

```
=MAX(D1:D100)
=MIN(E1:E100)
```

## Logical Functions

### IF

Conditional logic.

```
=IF(A1>100, "High", "Low")
=IF(B1="", "Empty", B1)
```

### AND / OR

Logical operations.

```
=AND(A1>0, A1<100)
=OR(B1="Yes", B1="Y")
```

## Text Functions

### CONCATENATE

Joins text.

```
=CONCATENATE(A1, " ", B1)
```

### LEN

Returns text length.

```
=LEN(A1)
```

### UPPER / LOWER

Changes case.

```
=UPPER(A1)
=LOWER(B1)
```

## Date Functions

### TODAY / NOW

Current date/time.

```
=TODAY()
=NOW()
```

### YEAR / MONTH / DAY

Extract date parts.

```
=YEAR(A1)
=MONTH(A1)
=DAY(A1)
```

## Lookup Functions

### VLOOKUP

Vertical lookup.

```
=VLOOKUP(A1, B1:D10, 2, FALSE)
```

### INDEX / MATCH

Advanced lookup.

```
=INDEX(C1:C10, MATCH(A1, B1:B10, 0))
```
