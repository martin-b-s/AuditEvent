# AuditEvent

## Examples

### Get

curl -X 'GET' \
  'https://localhost:7150/events' \
  -H 'accept: text/plain'

### Post

curl -X 'POST' \
  'https://localhost:7150/events' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '  {
    "id": "636fc316-4b93-499a-b9f5-6d22a6c7ccaa",
    "timestamp": "2025-06-02T19:07:33.3589569+08:00",
    "serviceName": "Test2",
    "eventType": "USER.DELETED",
    "payload": "{}"
  }'

## Assumptions
 - Id is unique


## TODO
- Proper http return codes
- read/write critical section instead of lock?
- better logging