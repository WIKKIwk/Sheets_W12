/**
 * Formula Usage Examples for W12C Sheets
 * 
 * This demonstrates the 100+ formulas available in W12C Sheets
 * Compatible with Excel and Google Sheets formulas
 */

// Mathematical Formulas
const mathExamples = {
    // Basic arithmetic
    sum: '=SUM(A1:A10)',
    average: '=AVERAGE(B1:B10)',
    count: '=COUNT(C1:C10)',
    max: '=MAX(D1:D10)',
    min: '=MIN(E1:E10)',

    // Advanced math
    round: '=ROUND(A1, 2)',
    abs: '=ABS(A1)',
    sqrt: '=SQRT(A1)',
    power: '=POWER(A1, 2)',
    mod: '=MOD(A1, 3)',
    product: '=PRODUCT(A1:A5)',
    sumproduct: '=SUMPRODUCT(A1:A5, B1:B5)',
};

// Statistical Formulas
const statisticalExamples = {
    median: '=MEDIAN(A1:A10)',
    mode: '=MODE(A1:A10)',
    stdev: '=STDEV(A1:A10)',
    variance: '=VAR(A1:A10)',
    sumif: '=SUMIF(A1:A10, ">100", B1:B10)',
    countif: '=COUNTIF(A1:A10, ">50")',
    averageif: '=AVERAGEIF(A1:A10, ">=100", B1:B10)',
    correlation: '=CORREL(A1:A10, B1:B10)',
    covariance: '=COVARIANCE(A1:A10, B1:B10)',
};

// Logical Formulas
const logicalExamples = {
    if: '=IF(A1>100, "High", "Low")',
    and: '=AND(A1>50, B1<100)',
    or: '=OR(A1>100, B1>100)',
    not: '=NOT(A1>100)',
    xor: '=XOR(A1>50, B1>50)',
    ifs: '=IFS(A1>100, "High", A1>50, "Medium", TRUE, "Low")',
    switch: '=SWITCH(A1, 1, "One", 2, "Two", 3, "Three", "Other")',
    iferror: '=IFERROR(A1/B1, "Division Error")',
    ifna: '=IFNA(VLOOKUP(A1, B:C, 2, FALSE), "Not Found")',
};

// Text Formulas
const textExamples = {
    len: '=LEN(A1)',
    upper: '=UPPER(A1)',
    lower: '=LOWER(A1)',
    trim: '=TRIM(A1)',
    concatenate: '=CONCATENATE(A1, " ", B1)',
    left: '=LEFT(A1, 5)',
    right: '=RIGHT(A1, 3)',
    mid: '=MID(A1, 2, 5)',
    find: '=FIND("text", A1)',
    search: '=SEARCH("word", A1)',
    replace: '=REPLACE(A1, 1, 3, "new")',
    substitute: '=SUBSTITUTE(A1, "old", "new")',
};

// Date/Time Formulas
const dateTimeExamples = {
    today: '=TODAY()',
    now: '=NOW()',
    year: '=YEAR(A1)',
    month: '=MONTH(A1)',
    day: '=DAY(A1)',
    hour: '=HOUR(A1)',
    minute: '=MINUTE(A1)',
    second: '=SECOND(A1)',
    date: '=DATE(2026, 1, 5)',
    time: '=TIME(15, 30, 0)',
    days: '=DAYS(A2, A1)',
    networkdays: '=NETWORKDAYS(A1, A2)',
};

// Lookup & Reference Formulas
const lookupExamples = {
    vlookup: '=VLOOKUP(A1, B:D, 2, FALSE)',
    hlookup: '=HLOOKUP(A1, B1:Z5, 3, FALSE)',
    index: '=INDEX(A1:D10, 2, 3)',
    match: '=MATCH(A1, B:B, 0)',
    offset: '=OFFSET(A1, 2, 3)',
    indirect: '=INDIRECT("A" & ROW())',
    row: '=ROW()',
    column: '=COLUMN()',
    rows: '=ROWS(A1:A10)',
    columns: '=COLUMNS(A1:D1)',
};

// Practical Examples
const practicalExamples = {
    // Calculate total with tax
    totalWithTax: '=A1 * 1.13',

    // Grade calculation
    grade: '=IFS(A1>=90, "A", A1>=80, "B", A1>=70, "C", A1>=60, "D", TRUE, "F")',

    // Full name from first and last
    fullName: '=CONCATENATE(A1, " ", B1)',

    // Days until deadline
    daysUntilDeadline: '=DAYS(B1, TODAY())',

    // Conditional sum
    conditionalSum: '=SUMIF(A:A, ">100", B:B)',

    // Find and calculate percentage
    percentage: '=ROUND((A1/B1)*100, 2)',

    // Lookup price
    lookupPrice: '=VLOOKUP(A1, Products!A:C, 3, FALSE)',

    // Running total
    runningTotal: '=SUM($A$1:A1)',
};

module.exports = {
    mathExamples,
    statisticalExamples,
    logicalExamples,
    textExamples,
    dateTimeExamples,
    lookupExamples,
    practicalExamples,
};
