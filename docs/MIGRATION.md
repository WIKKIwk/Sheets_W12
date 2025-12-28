# Migration Guide

## From Google Sheets

1. Export Google Sheet as CSV
2. Import CSV into W12C Sheets
3. Verify formulas
4. Test functionality

## From Excel

1. Save as CSV
2. Upload to W12C
3. Check formula compatibility

## Data Import

```bash
# Via API
curl -X POST http://localhost:8001/api/import \
  -F "file=@data.csv" \
  -H "Authorization: Bearer TOKEN"
```
