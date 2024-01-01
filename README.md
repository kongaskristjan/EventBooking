# EventBooker

## Postgres andmebaasi diagramm

```mermaid
erDiagram

    EVENT {
        integer id PK "Event id, primary key"
        text name "Event name"
        timestamp timestamp "Event timestamp"
        text location "Event location string"
        varchar(1000) info "Additional information"
    }
    PERSON {
        integer id PK "Person id, primary key"
        integer event_id FK "Event id the person is participating, foreign key"
        text first_name "First name"
        text last_name "Last name"
        text person_identification_number "Personal identification number"
        text payment_method "Payment method ('Cash'/'BankTransfer')"
        varchar(1500) info "Additional information"
    }
    COMPANY {
        integer id PK "Company id, primary key"
        integer event_id FK "Event id, foreign key"
        text name "Name of the company"
        text company_registration_number "Registration number of the company"
        integer n_participants "Number of participants"
        text payment_method "Payment method ('Cash'/'BankTransfer')"
        varchar(5000) info "Additional information"
    }
    EVENT ||--o{ PERSON : ""
    EVENT ||--o{ COMPANY : ""
```

## Setup

Lokaalseks testimiseks peab seadistama postgres andmebaasi. Selletarvis on olemas `docker-compose` setup, mille saab `EventBooking/` kaustast käivitada selliselt:

```bash
$ docker-compose up -d
```

Et õiged konfiguratsioonifailid üles leida, peab keskkonnamuutujatele õiged väärtused omistama:
```
ASPNETCORE_ENVIRONMENT=Development
```

Esmakordsel käivitamisel tuleb ka andmebaasi migratsioonid ära teha:

```bash
$ dotnet ef database update
```

Seejärel saab käivitada serveri:

```bash
$ dotnet watch
```
