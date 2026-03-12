# Haulmer.Payments

Servicio de procesamiento de pagos construido con **.NET 8**, **ASP.NET Core Web API**, **SQL Server**, **EF Core Code First** y **Docker**.

El proyecto fue desarrollado con foco en:

- arquitectura limpia
- reglas de negocio explícitas
- idempotencia
- trazabilidad funcional y técnica
- persistencia reproducible
- facilidad de ejecución local

---

## Stack

- .NET 8
- ASP.NET Core Web API
- EF Core
- SQL Server
- Docker
- Swagger / OpenAPI
- xUnit + FluentAssertions

---

## Estructura de la solución

- `src/Haulmer.Payments.Api`
- `src/Haulmer.Payments.Application`
- `src/Haulmer.Payments.Domain`
- `src/Haulmer.Payments.Infrastructure`
- `tests/Haulmer.Payments.Domain.UnitTests`
- `tests/Haulmer.Payments.Application.UnitTests`
- `tests/Haulmer.Payments.Api.IntegrationTests`

---

## Decisiones principales

### Arquitectura
La solución está separada por capas:

- **Domain**: entidades, invariantes y reglas de negocio
- **Application**: casos de uso, validaciones, queries y contratos
- **Infrastructure**: EF Core, repositorios, migrations y servicios externos simulados
- **Api**: controllers, contratos HTTP, middlewares y configuración

### Modelo de pagos
El proyecto usa **Model A**:

- el comercio informa el monto bruto a cobrar
- la plataforma calcula internamente fees e impuestos
- los cargos no aumentan el monto cobrado al cliente
- el sistema persiste tanto el **gross amount** como el **net amount**

### Trazabilidad
Se separan dos conceptos:

- **historial funcional**: `PaymentTransactionStatusHistories`
- **traza técnica**: `PaymentTraceLogs`

### Idempotencia
La idempotencia se controla por:

- `MerchantId`
- `IdempotencyKey`

Además, existe índice único en base de datos para reforzar esta regla.

### Autorizador simulado
El servicio externo actual es una simulación controlada:

- `GrossAmount <= 1000000` → `Approved`
- `GrossAmount > 1000000` → `Declined`

---

## Estrategia local de base de datos

La inicialización local se divide en tres pasos:

1. **Docker** levanta SQL Server
2. **EF Core Migrations** crea el esquema
3. un **script SQL seed** carga datos base de prueba

Con esto, el entorno queda simple, reproducible y fácil de evaluar.

---

## Levantar el proyecto rápido

### 1. Levantar SQL Server con Docker

Definir la contraseña del contenedor:

```bash
export SQLSERVER_SA_PASSWORD='HaulmerPayments_2026!'
```

Levantar el contenedor:

```bash
docker compose up -d
```

Verificar que esté corriendo:

```bash
docker ps
```

---

### 2. Aplicar migrations

Desde la raíz del repositorio:

```bash
dotnet ef database update \
  --project src/Haulmer.Payments.Infrastructure \
  --startup-project src/Haulmer.Payments.Api
```

Esto crea:

- la base `HaulmerPaymentsDb`
- el schema `payments`
- las tablas del modelo
- `__EFMigrationsHistory`

---

### 3. Ejecutar el seed

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
- canal
- adquirente
- pricing y configuración del merchant
- pagos de ejemplo
- historial de estados
- trazas técnicas
- registros de idempotencia

---

### 4. Levantar la API

```bash
dotnet run --project src/Haulmer.Payments.Api
```

Luego abrir Swagger en la URL que informa la aplicación al iniciar.

---

## Datos seed disponibles

### Configuración principal

- `merchantId = 1`
- `merchantBranchId = 1`
- `paymentMethodId = 1`
- `paymentChannelId = 1`
- `MerchantCode = MRC-001`
- `BranchCode = BR-001`
- `PaymentMethod = CARD`
- `PaymentChannel = ECOMMERCE`
- `Acquirer = SIM`
- `Currency = CLP`

### Pagos de ejemplo

El seed deja transacciones precargadas para facilitar pruebas de lectura:

- `TXN-DEMO-0001` → `Approved`
- `TXN-DEMO-0002` → `Declined`
- `TXN-DEMO-0003` → `Failed`

---

## Endpoints implementados

- `POST /payments`
- `GET /payments/{transactionId}`
- `GET /payments?merchant_id=...&status=...`

---

## Ejemplo de request para `POST /payments`

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

Resultado esperado:

- si `baseAmount + tipAmount <= 1000000`, el pago se aprueba
- si `baseAmount + tipAmount > 1000000`, el pago se rechaza

---

## Cómo validar rápidamente

### Crear pago
Probar en Swagger con el request anterior.

### Consultar por id
Usar un `PaymentTransactionId` existente del seed o de una transacción recién creada.

### Buscar pagos
Ejemplos:

- `GET /payments?merchant_id=1`
- `GET /payments?merchant_id=1&status=Approved`
- `GET /payments?merchant_id=1&status=Declined`
- `GET /payments?merchant_id=1&status=Failed`

---

## Pruebas automatizadas

Ejecutar todos los tests:

```bash
dotnet test
```

---

## Comandos útiles

### Crear migration

```bash
dotnet ef migrations add InitialCreate \
  --project src/Haulmer.Payments.Infrastructure \
  --startup-project src/Haulmer.Payments.Api \
  --output-dir Persistence/Migrations
```

### Aplicar migrations

```bash
dotnet ef database update \
  --project src/Haulmer.Payments.Infrastructure \
  --startup-project src/Haulmer.Payments.Api
```

### Listar migrations

```bash
dotnet ef migrations list \
  --project src/Haulmer.Payments.Infrastructure \
  --startup-project src/Haulmer.Payments.Api
```

---

## Troubleshooting rápido

### La base no existe
Aplicar migrations nuevamente:

```bash
dotnet ef database update \
  --project src/Haulmer.Payments.Infrastructure \
  --startup-project src/Haulmer.Payments.Api
```

### `POST /payments` falla por datos faltantes
Ejecutar el seed:

```text
scripts/db/seed/001_initial_seed.sql
```

### SQL Server no está disponible
Verificar contenedor:

```bash
docker ps
```

### La API no conecta a la base
Revisar:

- contenedor levantado
- puerto `14333`
- password configurada correctamente
- connection string en `appsettings.json`

---

## Estado actual

El proyecto deja implementado un flujo completo de prueba técnica con:

- creación de pagos
- consulta de pago por id
- consulta de pagos por merchant y estado
- migrations reproducibles
- seed local
- autorizador simulado
- tests unitarios
- validación end-to-end
