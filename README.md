# Implementing Business Logic

This module focuses on implementing business logic in services. You will learn how to define an interface. Then implement that interface in a service class. You will learn how to set up a test project to unit test the business logic implementation.

## Topics
- [Interfaces](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/types/interfaces) & [implementations](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/interfaces#interface-implementations)
- [Namespaces](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/types/namespaces)
- [Asynchronous programming](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/)
- Working with [collections](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/collections) & [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/linq/)
- Unit testing & integration testing with [NUnit](https://nunit.org/)
- NuGet management

## Prerequisites
You'll need to set up your machine to run .NET, including the C# 10.0 compiler. [Download](https://dotnet.microsoft.com/download/dotnet/6.0) & install the .NET 6 SDK.

You'll need a connection string to an [SQL Server database](https://www.microsoft.com/en-us/sql-server/developer-get-started/).

## Requirements

We are going to create an application that keeps track of the modes of transport that a company's employees use to commute to work daily. The full solution will include:
- Data project: defines the data structure
- Domain project: business logic implementations
- WebApplication project: includes the web API's & backoffice web application to manage the employee data
- Mobile app project: will be used by the employees to input the mode of transport for their daily commute

The data layer is already implemented in the previous module. Now we are going to implement the domain layer.
During development, run the included (unit) tests to get feedback on your progress. Once a test passes, move on the making the next test pass.
## Assignments

### Create a new class library project
Create a new class library project and name it `MyCommute.Domain`
This project will contain the domain logic for handling employees & commutes.

### Define interface IEmployeeService
The EmployeeService will handle CRUD operations related to the `MyCommute.Data.Entities.Employee` entity (further referred to as `Employee`). It also contains methods to fetch employee data. 
In project MyCommute.Domain, create a new directory `Services`.
In the `Services` directory, define an interface `IEmployeeService`.

`IEmployeeService` declares:
 - `GetAsync`: accepts no parameters, and returns a collection of all `Employee` entities in the database.
 - `GetByIdAsync`: accepts 1 parameters of type `Guid` and returns one `Employee` that matches the provided id.
 - `GetByEmailAsync`" accepts 1 parameter of type `string` and returns one `Employee` that matches the provided email address.
 - `AddAsync`: accepts 1 parameter of type `Employee` and returns the `Employee` entity after saving it to the database.
 - `UpdateAsync`: accepts 1 parameter of type `Employee` and returns the `Employee` entity after saving it to the database.
 - `DeleteAsync`: accepts 1 parameters of type `Guid` and returns type `bool` to indicate success after removing the `Employee` matching the provided id from the database.
 - `DeleteAsync`: accepts 1 parameters of type `string` and returns type `bool` to indicate success after removing the `Employee` matching the provided email address from the database.

### Implement IEmployeeService in EmployeeService
In the `Services` directory, create a new directory `Implementations`. In that directory, create a new class and name it `EmployeeService`.

`EmployeeService` implements interface `IEmployeeService`:
``` c#
public class EmployeeService : IEmployeeService
```

`EmployeeService` has a private, read-only field `dataContext` of type `MyCommute.Data.Entities.DataContext` (further referred to as `DataContext`).
```c#
private readonly DataContext dataContext;
```

The constructor of `EmployeeService` accepts 1 argument of type `DataContext`, and initializes the `dataContext` field.
```c#
public EmployeeService(DataContext context)
{
    dataContext = context;
}
```
The methods declared in `IEmployeeService` are implemented in `EmployeeService`:
- `GetAsync`: accepts no parameters, and returns a collection of all `Employee` entities in the database.
- `GetByIdAsync`: accepts 1 parameters of type `Guid` and returns one `Employee` that matches the provided id.
- `GetByEmailAsync`: accepts 1 parameter of type `string` and returns one `Employee` that matches the provided email address.
- `AddAsync`: accepts 1 parameter of type `Employee` and returns the `Employee` entity after saving it to the database.
- `UpdateAsync`: accepts 1 parameter of type `Employee` and returns the `Employee` entity after saving it to the database. Throws `EmployeeNotFoundException` when no matching `Employee` is found.
- `DeleteAsync`: accepts 1 parameters of type `Guid` and returns type `bool` to indicate success after removing the `Employee` matching the provided id from the database. Throws `EmployeeNotFoundException` when no matching `Employee` is found.
- `DeleteAsync`: accepts 1 parameters of type `string` and returns type `bool` to indicate success after removing the `Employee` matching the provided email address from the database. Throws `EmployeeNotFoundException` when no matching `Employee` is found.

### Define interface ICommuteService
The CommuteService will handle CRUD operations related to the `MyCommute.Data.Entities.Commute` entity (further referred to as `Commute`). It also contains methods to fetch commute data.
In the `Services` directory, define an interface `ICommuteService`.

`ICommuteService` declares:
 - `GetAsync`: accepts 1 parameters of type `Guid` and returns one `Commute` that matches the provided id.
 - `GetAllAsync`: accepts no parameters, and returns a collection of all `Commute` entities in the database.
 - `GetByUserIdAsync`: accepts 1 parameters of type `Guid` and returns a collection of all `Commute` entities related to the `Employee` that matches the provided id.
 - `AddAsync`: accepts 1 parameter of type `Commute` and returns the `Commute` entity after saving it to the database.
 - `UpdateAsync`: accepts 1 parameter of type `Commute` and returns the `Commute` entity after saving it to the database.
 - `DeleteAsync`: accepts 1 parameters of type `Guid` and returns type `bool` to indicate success after removing the `Commute` matching the provided id from the database.

### Implement ICommuteService in CommuteService
In the `Implementations` directory, create a new class and name it `CommuteService`.

`CommuteService` implements interface `ICommuteService`.

`CommuteService` has a private, read-only field `dataContext` of type `DataContext`.
The constructor of `CommuteService` accepts 1 argument of type `DataContext`, and initializes the `dataContext` field.

The methods declared in `ICommuteService` are implemented in `CommuteService`:
- `GetAsync`: accepts 1 parameters of type `Guid` and returns one `Commute` that matches the provided id. Throws `CommuteNotFoundException` when no matching `Commute` is found.
- `GetAllAsync`: accepts no parameters, and returns a collection of all `Commute` entities in the database.  Throws `CommuteNotFoundException` when no matching `Commute` is found.
- `GetByUserIdAsync`: accepts 1 parameters of type `Guid` and returns a collection of all `Commute` entities related to the `Employee` that matches the provided id.
- `AddAsync`: accepts 1 parameter of type `Commute` and returns the `Commute` entity after saving it to the database.
- `UpdateAsync`: accepts 1 parameter of type `Commute` and returns the `Commute` entity after saving it to the database. Throws `CommuteNotFoundException` when no matching `Commute` is found.
- `DeleteAsync`: accepts 1 parameters of type `Guid` and returns type `bool` to indicate success after removing the `Commute` matching the provided id from the database.  Throws `CommuteNotFoundException` when no matching `Commute` is found.

### Add Date property to the Commute class and generate a migration
A frequently occurring task during development is updating the properties of an existing entity, and generating a migration to update the database schema.
While implementing the CommuteService, we realised that the `Commute` entity is missing a timestamp to indicate which date the commute relates to. 
Add a new property `Date` with type `DateTime`. Then generate a migration.

### Create Address model
Before starting implementation of the GeoCodeService, first create a new Address [record](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record)

Properties:
- Street ([string](https://docs.microsoft.com/en-us/dotnet/api/system.string))
- HouseNumber ([string](https://docs.microsoft.com/en-us/dotnet/api/system.string))
- ZipCode ([string](https://docs.microsoft.com/en-us/dotnet/api/system.string))
- City ([string](https://docs.microsoft.com/en-us/dotnet/api/system.string))
- Country ([string](https://docs.microsoft.com/en-us/dotnet/api/system.string))

### Define interface IGeoCodeService
The GeoCodeService features methods for geocoding an address to geographic coordinates (longitude & latitude), and the reverse operation of retrieving an address from geographical coordinates.
In the `Services` directory, define an interface `IGeoCodeService`.

`IGeoCodeService` declares:
- `GetCoordinatesForAddressAsync`: accepts 1 parameter of type `Address` and returns one `NetTopologySuite.Geometries.Point` (further referred to as `Point`).
- `GetAddressForCoordinatesAsync`: accepts 1 parameter of type `Point` and returns one `Address`.

### Implement IGeoCodeService in GeoCodeService
A number of geocoding & reverse geocoding API's are publicly available, as well as NuGet packages to consume those API's. You are free to choose which API and/or NuGet package you want to use.

The methods declared in `IGeoCodeService` are implemented in `GeoCodeService`:
- `GetCoordinatesForAddressAsync`: accepts 1 parameter of type `Address` and returns one `Point`. Throws `ForwardGeoCodeFailedException` when no geographical coordinates could be found for the given address.
- `GetAddressForCoordinatesAsync`: accepts 1 parameter of type `Point` and returns one `Address`. Throws `ReverseGeoCodeFailedException` when the given geographical coordinates could be resolved to an address.