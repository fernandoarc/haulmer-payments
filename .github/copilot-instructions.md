# Copilot Instructions for Haulmer.Payments

You are assisting in the development of **Haulmer.Payments**, a payment processing service built with **.NET 8**, **ASP.NET Core Web API**, **SQL Server**, and **EF Core Code First**.

Your role is to help produce code, structure, and technical decisions that are consistent with the architecture, domain model, coding standards, testing approach, and workflow conventions defined for this repository.

---

## Project Context

This solution is a **Payment Processing Service** for the **Chilean market only**.

The service is responsible for:

- receiving payment requests from multiple merchants
- validating merchant and payment configuration
- applying business rules
- calculating commissions and charges
- sending the payment to an external acquirer
- recording the full transaction lifecycle
- supporting complete traceability and idempotency

This project must remain:

- interview-friendly
- readable
- maintainable
- scalable
- professionally structured

---

## Main Assumptions

- Market scope is **Chile only**
- Main currency is **CLP**
- Merchant tax identifier is based on **RUT**
- The system uses **EF Core Code First**
- SQL Server is the relational database
- Payment flow is initially synchronous
- No physical deletes should be used for transactional history
- Transaction status changes must be recorded historically
- Technical traceability must be stored separately from functional transaction history
- The architecture must follow **Clean Architecture**
- The codebase must respect **SOLID principles**
- The API style must use **Controllers**, not Minimal APIs

### Commission model

The project uses **Model A**:

- the merchant sends the **gross amount to charge the customer**
- the system calculates fees internally
- fees do **not** increase the amount charged to the customer
- the platform calculates internal charges and derives the net amount to settle to the merchant

### Tips support

Tips are supported.

Rules:

- `GrossAmount = BaseAmount + TipAmount`
- processor fees are calculated over `GrossAmount`
- `BaseAmount > 0`
- `TipAmount >= 0`

---

## Solution Structure

The repository follows this structure:

- `src/Haulmer.Payments.Api`
- `src/Haulmer.Payments.Application`
- `src/Haulmer.Payments.Domain`
- `src/Haulmer.Payments.Infrastructure`
- `tests/Haulmer.Payments.Domain.UnitTests`
- `tests/Haulmer.Payments.Application.UnitTests`
- `tests/Haulmer.Payments.Api.IntegrationTests`

---

## Domain Structure

The Domain project is organized by **business concept**, not by technical artifact type.

Preferred structure:

- `Merchants/`
- `Catalogs/`
- `MerchantSetup/`
- `Payments/`
- `Common/` only when truly necessary

Rules:

- do not create a generic `Enums/` folder for domain enums
- do not centralize entities, enums, and value objects by technical category if they clearly belong to a business concept
- keep each enum close to its related domain concept
- keep namespaces aligned with the business concept folder
- prefer organizing domain code by vertical slice / business concept

Examples:

- `Haulmer.Payments.Domain.Merchants`
- `Haulmer.Payments.Domain.Catalogs`
- `Haulmer.Payments.Domain.MerchantSetup`
- `Haulmer.Payments.Domain.Payments`

---

## Layer Responsibilities

### Domain
Contains:
- entities
- enums
- value objects
- domain rules
- domain invariants

Rules:
- must not depend on Infrastructure
- should stay framework-light whenever possible
- should contain the core business language
- should model the business clearly and explicitly
- should not contain EF Core data annotations
- should not contain HTTP concerns
- should not contain database-specific behavior

### Application
Contains:
- use cases
- commands
- queries
- DTOs
- interfaces
- validators
- orchestration logic

Rules:
- depends on Domain only
- must not contain persistence implementation details
- should coordinate business flow without leaking HTTP concerns
- should orchestrate creation/update flows by calling domain behavior
- should provide time/date values to domain entities instead of forcing entities to call `DateTime.UtcNow`

### Infrastructure
Contains:
- EF Core persistence
- DbContext
- entity configurations
- migrations
- repositories
- external service clients
- logging integrations

Rules:
- implements interfaces defined in Application
- should keep persistence concerns isolated
- should use Fluent API for entity mapping
- should configure identity/autoincrement keys here, not in Domain
- should configure indexes, decimal precision, constraints, and relationships explicitly

### Api
Contains:
- controllers
- dependency injection setup
- middleware
- filters
- request/response contracts

Rules:
- must stay thin
- must not contain business logic
- should delegate processing to Application layer
- must use Controllers, not Minimal APIs

---

## API Style

Use **ASP.NET Core Controllers**, not Minimal APIs.

Rules:
- do not generate Minimal API endpoint mappings in `Program.cs`
- prefer controllers with explicit routes
- prefer request/response DTOs
- keep `Program.cs` focused on configuration and dependency injection
- use controllers for maintainability, readability, and scalability

---

## API Project Structure

Within the API project, prefer this structure:

- `Controllers/`
- `Contracts/Requests/`
- `Contracts/Responses/`
- `Extensions/`
- `Middlewares/`
- `Filters/`

Rules:
- do not place business logic in controllers
- do not implement endpoint behavior directly in `Program.cs`
- keep controllers focused on transport concerns only

---

## Architecture Principles

Always follow these principles:

- prefer **Clean Architecture**
- favor **high cohesion** and **low coupling**
- keep responsibilities clearly separated
- avoid business logic inside controllers
- avoid leaking database concerns into Application or Api layers
- optimize for **maintainability**, **traceability**, and **evolution**
- prefer explicit, readable code over clever code
- model the domain clearly using meaningful names
- keep the solution simple, but not simplistic
- respect **SOLID**
- avoid overengineering
- make pragmatic decisions that fit an interview exercise while still looking production-oriented

---

## Domain Modeling Guidelines

When generating entities, enums, value objects, handlers, or contracts, align with the payment domain already defined.

### Merchants
- Merchant
- MerchantBranch
- MerchantStatus
- MerchantBranchStatus

### Catalogs
- PaymentMethod
- PaymentMethodStatus
- PaymentChannel
- PaymentChannelStatus
- Acquirer
- AcquirerStatus

### MerchantSetup
- MerchantPaymentMethod
- MerchantPaymentMethodStatus
- MerchantAcquirerConfiguration
- MerchantAcquirerConfigurationStatus
- MerchantPricing
- MerchantPricingStatus

### Payments
- PaymentTransaction
- PaymentTransactionStatus
- PaymentTransactionStatusHistory
- PaymentIdempotency
- PaymentIdempotencyStatus
- PaymentTraceLog
- PaymentRequest
- TraceSeverity

---

## Current Domain Decisions Already Taken

These are already established and must be respected.

### Merchant entity conventions
- `MerchantId` must be of type `long`
- it is an identity/autoincrement key generated by persistence
- Domain entities must not require identity values in their factory if the key is DB-generated
- `Merchant` uses a **private parameterless constructor** for EF Core
- `Merchant` uses a **static factory method** for creation
- `Merchant` should not use a public constructor for normal creation flow
- `Merchant` stores both UTC and local timestamps:
  - `CreatedAtUtc`
  - `CreatedAtLocal`
  - `UpdatedAtUtc`
  - `UpdatedAtLocal`
- `Merchant` uses `CompanyRut` as the Chilean company tax identifier
- `TaxId` is not the preferred property name in this project for a Chilean merchant company
- domain behavior must validate required fields
- domain behavior must be unit testable

### MerchantBranch decision
- `MerchantBranch` already represents the branch/sucursal concept
- do not introduce another separate branch entity
- do not modify `MerchantBranch` for now unless explicitly requested

### Domain entity design rules
When generating Domain entities:

- prefer **private setters**
- use a **private parameterless constructor** for EF Core
- prefer **static factory methods** for entity creation instead of public constructors
- do not use EF Core data annotations inside Domain
- keep entities persistence-friendly for Code First, but free from infrastructure concerns
- apply only domain-relevant validations inside the entity
- pass timestamps as parameters when needed instead of calling `DateTime.UtcNow` directly inside business methods
- use **identity/autoincrement numeric keys** when the domain has already chosen them
- use `long` for main entity identifiers unless a different choice has been explicitly defined

### Validation scope inside Domain
Inside Domain entities:
- validate required values
- validate non-default date values when dates are required
- validate invariant consistency
- do not add heavy infrastructure concerns
- do not add regex-heavy or external validation unless explicitly requested
- do not query the database from the entity

### MerchantSetup date validations
For MerchantSetup entities, validate both:
- business-effective dates such as `EnabledFromUtc`, `EnabledFromLocal`, `ValidFromUtc`, `ValidFromLocal`
- technical creation dates such as `CreatedAtUtc`, `CreatedAtLocal`

When generating tests for `ArgumentException`, prefer validating:
- `exception.Message.Should().StartWith("...")`
- `exception.ParamName.Should().Be("...")`

Do not rely on fragile full-string comparisons when the exception also includes parameter names.

---

## Important domain rules

Respect these rules whenever relevant:

- merchant must exist
- payment method must exist
- payment method must be enabled for the merchant
- merchant must have valid acquirer configuration
- amount must be greater than zero
- currency must be informed
- transaction must start in `PENDING`
- transaction status history is append-only
- terminal states must not transition again in normal flow
- idempotency must be enforced by merchant and idempotency key
- trace logs are separate from transaction status history

---

## Transaction statuses

Supported statuses:

- `PENDING`
- `PROCESSING`
- `APPROVED`
- `DECLINED`
- `FAILED`

Semantics:

- `DECLINED` = functional or business rejection
- `FAILED` = technical or system failure

Terminal states:
- `APPROVED`
- `DECLINED`
- `FAILED`

Normal transitions out of terminal states must not be allowed.

---

## Amount model

Use these monetary fields in `PaymentTransaction`:

- `BaseAmount`
- `TipAmount`
- `GrossAmount`
- `FeeAmount`
- `VatAmount`
- `TaxAmount`
- `OtherChargesAmount`
- `NetAmount`

Rules:

- `BaseAmount > 0`
- `TipAmount >= 0`
- `GrossAmount = BaseAmount + TipAmount`

---

## Persistence Guidelines

Use **EF Core Code First**.

### Mapping rules
- prefer `IEntityTypeConfiguration<T>`
- keep entity configuration out of entities
- explicitly configure:
  - table names
  - keys
  - required fields
  - max lengths
  - indexes
  - relationships
  - decimal precision
- use a single consistent naming convention across the whole solution
- keep persistence configuration readable and explicit
- identity/autoincrement keys must be configured in Infrastructure, not in Domain

### SQL Server considerations
- use appropriate indexes for high-demand queries
- use decimal precision explicitly for monetary fields
- avoid storing sensitive card data
- never store full PAN or CVV
- only persist masked PAN, last4, and non-sensitive metadata

### Dates and time
Prefer storing:
- UTC timestamps as source of truth
- local timestamp when required for business traceability
- UTC offset when needed in transactional contexts

---

## Testing Strategy

The tests structure must reflect the architectural intent of `src`.

Preferred structure:

- `tests/Haulmer.Payments.Domain.UnitTests`
- `tests/Haulmer.Payments.Application.UnitTests`
- `tests/Haulmer.Payments.Api.IntegrationTests`

### Test folder organization
Inside each test project, organize tests by the same business concepts as the source code.

Examples:
- `tests/Haulmer.Payments.Domain.UnitTests/Merchants/MerchantTests.cs`
- `tests/Haulmer.Payments.Domain.UnitTests/Merchants/MerchantBranchTests.cs`
- `tests/Haulmer.Payments.Domain.UnitTests/Payments/PaymentTransactionTests.cs`

### Test project responsibilities

#### Domain.UnitTests
Use for:
- entities
- value objects
- domain rules
- invariants
- factory methods
- state transitions

Rules:
- do not mock unless strictly necessary
- do not use database access
- keep tests pure and fast
- test behavior, not trivial getters/setters

#### Application.UnitTests
Use for:
- handlers
- validators
- orchestrators
- application services

Rules:
- mocking is allowed when needed
- test use case orchestration and validation logic

#### Api.IntegrationTests
Use for:
- controllers
- routing
- serialization
- pipeline integration
- status codes
- request/response contracts

Rules:
- these are not unit tests
- use integration style when necessary

### Unit test conventions
When generating unit tests:

- use **xUnit**
- use **FluentAssertions** if available
- prefer Arrange / Act / Assert
- use descriptive names:
  - `MethodName_Should_DoSomething_When_Condition`
- create tests alongside the evolution of domain classes
- for Domain entities, create the test file as soon as the class has meaningful behavior
- avoid integration behavior inside Domain.UnitTests

### Merchant tests expectations
When generating tests for `Merchant`, cover at least:
- valid creation
- required field validations
- required timestamp validations
- initial status
- contact update behavior
- status transitions
- idempotent behavior when already in the same status
- updated timestamps when changes occur

### Payments tests expectations
When generating tests for `Payments`:
- validate creation invariants thoroughly
- validate status transitions explicitly
- validate terminal-state protection
- validate required timestamps
- validate required identifiers
- validate amount consistency
- validate correlation identifiers where applicable

---

## API Guidelines

When generating endpoints:

- keep controllers minimal
- use clear request and response DTOs
- validate inputs before processing
- return appropriate HTTP status codes
- do not expose internal implementation details
- design endpoints to be understandable and stable

Expected endpoints include:

- `POST /payments`
- `GET /payments/{transaction_id}`
- `GET /payments?merchant_id=...&status=...`

---

## Logging and Traceability

Traceability is a first-class concern.

Always design with:

- correlation id support
- structured logs
- clear event names
- separation between:
  - functional status history
  - technical trace log

Trace logs should help reconstruct the end-to-end flow of a transaction.

The system must support:
- transaction-level traceability
- merchant-level traceability
- timestamp traceability in UTC and local time when relevant

---

## Performance Guidelines

Assume this service operates under **high demand**.

Therefore:

- avoid unnecessary joins on hot paths
- keep current transaction status as a snapshot in the main transaction table
- keep calculated monetary values persisted in the transaction
- avoid overengineering, but do not ignore scale
- design for future optimization without harming readability
- use asynchronous APIs properly
- avoid loading more data than necessary

---

## Coding Style

Generate code that is:

- clear
- explicit
- production-oriented
- testable
- easy to maintain

Prefer:

- small focused classes
- meaningful method names
- explicit dependencies through constructor injection
- cancellation token support where appropriate
- guard clauses for validation
- concise comments only when they add value

Avoid:

- overly generic abstractions with no clear benefit
- premature microservice decomposition
- hidden magic behavior
- giant classes with mixed responsibilities
- logic duplication
- dead code
- placeholder comments like "TODO: implement later" unless explicitly requested

---

## Naming Conventions

Use clear English names in code.

Rules:

- use meaningful domain-oriented names
- avoid abbreviations unless they are industry-standard
- prefer singular names for entities and classes
- keep namespaces aligned with project structure
- prefer business-concept folders over technical-type folders inside Domain
- when referring to Chilean tax identity for companies, prefer `CompanyRut` if the concept is specifically a Chilean company RUT
- for payer identity in payments, use `PayerRut` in the Chilean market context

---

## Git Workflow

This repository follows **Git Flow**.

Branch guidelines:

- `main` contains stable production-ready code
- `develop` contains integration work
- feature branches must branch from `develop`

Use feature branch names like:

- `feature/HMR-001-add-domain-entities`
- `feature/HMR-002-add-merchant-setup-model`
- `feature/HMR-003-add-payment-transaction-model`

Rules:

- do not suggest direct work on `main` except for exceptional cases explicitly indicated by the developer
- keep names professional and descriptive
- do not use the word **bootstrap** in generated names

### Ticket naming convention
Use identifiers like:

- `HMR-001`
- `HMR-002`
- `HMR-003`

---

## Output Expectations

When generating files or code:

- produce complete code whenever possible
- keep namespaces aligned with project structure
- use .NET 8 compatible patterns
- keep the solution ready for EF Core migrations
- keep the project interview-friendly, readable, and professional

When generating test files:
- place them in the matching folder in the correct test project
- keep naming consistent with the source structure
- do not generate tests in the wrong project

---

## When Generating Code

Always prefer:

1. consistency with the existing architecture
2. correctness of the domain model
3. maintainability
4. traceability
5. performance-aware design

When in doubt:

- choose the simpler design that still respects the architecture
- keep domain rules explicit
- avoid mixing concerns across layers
- do not break already established design decisions for the current entity patterns

---

## When Proposing Improvements

You may suggest:

- better separation of responsibilities
- missing validations
- better entity relationships
- indexes for query performance
- clearer naming
- stronger traceability
- safer persistence decisions
- better test coverage strategy
- more consistent domain structure

But do not introduce unrelated complexity.