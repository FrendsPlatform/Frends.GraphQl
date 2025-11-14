# Changelog

## [2.0.0] - 2025-11-14
### Changed
- **BREAKING**: Variable values now support complex types (objects, arrays) instead of only strings. Variables are properly serialized without double-serialization.
- GET method now uses JSON serialization for variables instead of manual string construction.
- GraphQL response errors are now properly detected. Result.Success is set to false when the response contains errors, and an exception is thrown when ThrowErrorOnFailure is enabled.

### Added
- Support for operationName field to specify which operation to execute when a query contains multiple operations.
- Support for extensions field for protocol extensions and additional metadata.

## [1.1.0] - 2025-07-31
### Changed
- Make default error message empty by default

## [1.0.0] - 2025-06-03

### Added

- Initial implementation
