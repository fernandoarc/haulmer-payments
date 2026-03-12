# Haulmer.Payments

Servicio de procesamiento de pagos desarrollado con **.NET 8**, **ASP.NET Core Web API**, **SQL Server** y **EF Core Code First**.

El proyecto fue construido como una **prueba técnica** con foco en:

- modelado de dominio claro
- arquitectura limpia
- trazabilidad funcional y técnica
- idempotencia
- mantenibilidad
- facilidad de ejecución local

---

## Objetivo del proyecto

`Haulmer.Payments` permite:

- recibir solicitudes de pago de distintos comercios
- validar la configuración del merchant y del medio de pago
- calcular comisiones y cargos internos
- enviar la transacción a un autorizador simulado
- registrar el ciclo completo de la transacción
- mantener trazabilidad e historial de estados

El alcance actual está orientado al **mercado chileno**, usando **RUT** y **CLP** como contexto principal.

---

## Stack tecnológico

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **Docker**
- **Swagger / OpenAPI**
- **xUnit + FluentAssertions**

---

## Estrategia local de base de datos

La estrategia local de base de datos está separada en tres partes:

1. **Docker** levanta una instancia local de SQL Server.
2. **EF Core Migrations** crea y versiona el esquema de base de datos.
3. Un **script SQL de seed** carga los datos mínimos para probar el flujo localmente.

Esta separación permite que el entorno sea:

- reproducible
- fácil de entender
- fácil de revisar en una entrevista
- simple de resetear desde cero

---

## Arquitectura de la solución

La solución sigue una estructura basada en **Clean Architecture**:

- `src/Haulmer.Payments.Api`
- `src/Haulmer.Payments.Application`
- `src/Haulmer.Payments.Domain`
- `src/Haulmer.Payments.Infrastructure`
- `tests/Haulmer.Payments.Domain.UnitTests`
- `tests/Haulmer.Payments.Application.UnitTests`
- `tests/Haulmer.Payments.Api.IntegrationTests`

### Responsabilidades por capa

#### Domain
Contiene:
- entidades
- enums
- reglas de negocio
- invariantes

#### Application
Contiene:
- casos de uso
- commands
- queries
- validaciones
- interfaces
- lógica de orquestación

#### Infrastructure
Contiene:
- `DbContext`
- configuraciones EF Core
- migrations
- repositorios
- integración con servicios externos simulados

#### Api
Contiene:
- controllers
- contratos request/response
- middlewares
- configuración de Swagger
- punto de extensión para autenticación/autorización futura

---

## Estructura general del modelo de datos

El modelo se organiza en grupos de tablas:

### 1. Catálogos / datos maestros
- `payments.Merchants`
- `payments.MerchantBranches`
- `payments.PaymentMethods`
- `payments.PaymentChannels`
- `payments.Acquirers`

### 2. Configuración por merchant
- `payments.MerchantPaymentMethods`
- `payments.MerchantAcquirerConfigurations`
- `payments.MerchantPricings`

### 3. Flujo transaccional
- `payments.PaymentTransactions`
- `payments.PaymentRequests`
- `payments.PaymentIdempotencies`

### 4. Historial funcional
- `payments.PaymentTransactionStatusHistories`

### 5. Trazabilidad técnica
- `payments.PaymentTraceLogs`

### Decisiones relevantes del diseño
- El esquema SQL usa el schema **`payments`**
- La tabla principal de transacción guarda el **estado actual** como snapshot
- El historial de estados es **append-only**
- La trazabilidad técnica se guarda **separada** del historial funcional
- La idempotencia se maneja en una tabla dedicada
- El esquema es generado mediante **EF Core Migrations**

---

## Prerrequisitos

Antes de ejecutar el proyecto localmente, se requiere:

- Docker disponible localmente
- .NET SDK 8 instalado
- acceso a terminal desde la raíz del repositorio
- una herramienta para ejecutar el script SQL seed, por ejemplo:
  - SQL Server Management Studio
  - Azure Data Studio
  - extensión SQL de VS Code
  - `sqlcmd`

---

## Levantar SQL Server con Docker

El proyecto incluye un `docker-compose.yml` que levanta SQL Server localmente.

### 1. Definir la contraseña del usuario `sa`

```bash
export SQLSERVER_SA_PASSWORD='HaulmerPayments_2026!'
```

Levantar el contenedor:
```bash
docker compose up -d
```
Verificar que esté arriba:
```bash
docker ps
```

## 2. Aplicar migrations

Desde la raíz del repositorio:

```bash
dotnet ef database update \
  --project src/Haulmer.Payments.Infrastructure \
  --startup-project src/Haulmer.Payments.Api
```
Esto crea:

- la base HaulmerPaymentsDb
- el schema payments
- las tablas del modelo

## 3. Ejecutar el seed

Ruta del archivo:

```text
scripts/db/seed/001_initial_seed.sql
```

Ejecutar el script sobre la base:

```text
HaulmerPaymentsDb
```

El seed carga:

- merchant

- sucursal

- medio de pago

- canal de pago

- adquirente

- configuración del merchant

- pricing

- pagos de ejemplo

- historial de estados

- trazas técnicas

- registros de idempotencia

## 4. Levantar la API
```bash
dotnet run --project src/Haulmer.Payments.Api
```

## 5. Probar en Swagger

Una vez levantada la API, abrir Swagger en la URL que informa la aplicación al iniciar.

### Datos de prueba cargados por el seed
#### Configuración principal

- MerchantCode: MRC-001

- BranchCode: BR-001

- PaymentMethod: CARD

- PaymentChannel: ECOMMERCE

- Acquirer: SIM

- Currency: CLP

#### Comportamiento del autorizador simulado

- GrossAmount <= 1000000 → Approved

- GrossAmount > 1000000 → Declined

#### Pagos de ejemplo incluidos

- 1 pago Approved

- 1 pago Declined

- 1 pago Failed

#### Ejemplo de request para POST /payments
```json
{
  "merchantId": 1,
  "merchantBranchId": 1,
  "paymentMethodId": 1,
  "paymentChannelId": 1,
  "idempotencyKey": "IDEMP-POST-0001",
  "baseAmount": 10000,
  "tipAmount": 1000,
  "currency": "CLP",
  "payerFullName": "Juan Pérez",
  "payerRut": "11111111-1",
  "requestPayloadJson": "{\"orderId\":\"ORDER-1001\",\"source\":\"swagger\"}"
}
```

### Endpoints esperados

- POST /payments

- GET /payments/{transaction_id}

- GET /payments?merchant_id=...&status=...

### Ejecutar pruebas
```bash
dotnet test
```

### Comandos útiles
Crear migration
```bash
dotnet ef migrations add InitialCreate \
  --project src/Haulmer.Payments.Infrastructure \
  --startup-project src/Haulmer.Payments.Api \
  --output-dir Persistence/Migrations
```

Aplicar migrations
```bash
dotnet ef database update \
  --project src/Haulmer.Payments.Infrastructure \
  --startup-project src/Haulmer.Payments.Api
```
Listar migrations
```bash
dotnet ef migrations list \
  --project src/Haulmer.Payments.Infrastructure \
  --startup-project src/Haulmer.Payments.Api
```

### Troubleshooting rápido
#### La base no existe

Aplicar migrations:

```bash
dotnet ef database update \
  --project src/Haulmer.Payments.Infrastructure \
  --startup-project src/Haulmer.Payments.Api
```

POST /payments falla por configuración faltante

Ejecutar el seed:
```bash
scripts/db/seed/001_initial_seed.sql
```

#### SQL Server no está disponible

Verificar contenedor:
```bash
docker ps
```

#### La API no conecta a la base

Revisar:

- contenedor levantado

- puerto 14333

- password configurada correctamente

- connection string en appsettings.json