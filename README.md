### Domain-Driven Design sample

This project is a sample of .NET implementation of a medical prescription model using the approach "Domain-Driven Design" (DDD) and the architectural pattern "Command and Query Responsibility Segregation" (CQRS).

**Points of interest**

The goal of the project is to test and provide some .NET implementations of core concepts of DDD (entities, value objects, domain events, ...) and CQRS (commands, queries, command and query handlers, ...) on the basis of a concrete model.

Only a lightweight version of CQRS (no data projection, one data store) has been considered.

**Model**

The envisaged model reflects the lifecycle of a medical prescription and, in particular, of a pharmaceutical prescription. This lifecycle can be resumed by the following diagram.

The current model only takes into account use cases related to the prescriber : creation and revocation of a prescription.

_Command Model_

The command model takes various forms depending on the observed layer :

- The domain model represents the business model. It incorporates both behavior and data, and is composed of entities and value objects. The internal state of entities is fully encapsulated (no public getters and setters).
- The state model represents the state of the domain model. It incorporates only data and is composed of simple objects with public getters and setters.
- The data model is represented by the database. Sql scripts to generate the database (Sql Server and Oracle) are provided in the project "DDD.HealthcareDelivery.Application.IntegrationTests".

The mapping between the domain model and the state model has been inspired from the Memento pattern to capture the internal state of the domain model without violating encapsulation (method ToState()). The mapping between the state model and the data model has been realized by using Entity Framework (Code First, configuration classes).

The interactions between main components on the command-side can be represented as follows :

_Query Model_

As mentioned above, command and query data stores are not differentiated but the architecture on the query-side is simplified (as shown on the following diagram). The query-side is composed of simple Data Transfer Objects mapped to the database by using the Micro ORM Dapper.

**Projects**

Libraries are distributed by component (bounded context) and then by layer :

- The "Core" libraries include the core components (Entity, ValueObject, CommandHandler, ...) necessary to implement the approach DDD according to the architectural pattern CQRS.
- The "Common" libraries include common components (EmailAddress, FullName, ...) that can be reused in multiple bounded contexts (shared kernel).
- The "HealthcareDelivery" libraries include components related to the context of healthcare delivery.

The application layer can be tested by using the project "DDD.HealthcareDelivery.Application.IntegrationTests".

