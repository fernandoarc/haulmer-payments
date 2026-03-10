# Copilot Instructions for Haulmer.Payments

You are assisting in the development of **Haulmer.Payments**, a payment processing service built with **.NET 8**, **ASP.NET Core Web API**, **SQL Server**, and **EF Core Code First**.

Your role is to help produce code, structure, and technical decisions that are consistent with the architecture, domain model, and coding standards defined for this repository.

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

### Main assumptions

- Market scope is **Chile only**
- Main currency is **CLP**
- Merchant tax identifier is based on **RUT**
- The system uses **Code First** with EF Core
- SQL Server is the relational database
- Payment flow is initially synchronous
- No physical deletes should be used for transactional history
- Transaction status changes must be recorded historically
- Technical traceability must be stored separately from functional transaction history
- Commission model is **Model A**
  - the merchant sends the **gross amount to charge the customer**
  - the system calculates fees internally
  - fees do **not** increase the amount charged to the customer
- Tips are supported
  - `GrossAmount = BaseAmount + TipAmount`
  - processor fees are calculated over `GrossAmount`

## Solution Structure

The repository follows this structure:

- `src/Haulmer.Payments.Api`
- `src/Haulmer.Payments.Application`
- `src/Haulmer.Payments.Domain`
- `src/Haulmer.Payments.Infrastructure`
- `tests/Haulmer.Payments.UnitTests`

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

## Architecture Principles

Always follow these principles:

- prefer **clean architecture**
- favor **high cohesion** and **low coupling**
- keep responsibilities clearly separated
- avoid anemic controller design with business logic inside controllers
- avoid leaking database concerns into Application or Api layers
- optimize for **maintainability**, **traceability**, and **evolution**
- prefer explicit, readable code over clever code
- model the domain clearly using meaningful names

## Domain Modeling Guidelines

When generating entities, configurations, handlers, or contracts, align with the payment domain already defined.

Key concepts include:

- Merchant
- MerchantBranch
- PaymentMethod
- PaymentChannel
- Acquirer
- MerchantPaymentMethod
- MerchantAcquirerConfiguration
- MerchantPricing
- PaymentTransaction
- PaymentTransactionStatusHistory
- PaymentIdempotency
- PaymentTraceLog
- PaymentRequest

### Important domain rules

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

### Transaction statuses

Supported statuses:

- `PENDING`
- `PROCESSING`
- `APPROVED`
- `DECLINED`
- `FAILED`

Semantics:

- `DECLINED` = functional or business rejection
- `FAILED` = technical or system failure

### Amount model

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
- use snake_case or lower table naming only if the project standard explicitly defines it
- otherwise keep a single naming convention across the whole solution

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
- UTC offset when needed

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

## Naming Conventions

Use clear English names in code.

### Branch and ticket convention

Use identifiers like:

- `HMR-001`
- `HMR-002`

When suggesting branch names, follow this pattern:

- `feature/HMR-001-initialize-solution-structure`
- `feature/HMR-002-add-merchant-entities`
- `feature/HMR-003-configure-payment-transaction`

Do **not** use the word **bootstrap** in generated names.

## When generating code

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

## When proposing improvements

You may suggest:
- better separation of responsibilities
- missing validations
- better entity relationships
- indexes for query performance
- clearer naming
- stronger traceability
- safer persistence decisions

But do not introduce unrelated complexity.

## Output expectations

When generating files or code:

- produce complete code whenever possible
- keep namespaces aligned with project structure
- use .NET 8 compatible patterns
- keep the solution ready for EF Core migrations
- keep the project interview-friendly, readable, and professional