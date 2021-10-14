using System.Collections;

namespace MyCommute.Tests.IntegrationTests;

[TestFixture]
public class EmployeeServiceTests
{
    private IEmployeeService employeeService;
    
    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        var dataContext = new TestDataContext();
        employeeService = new EmployeeService(dataContext);
        
        // Seed database
        var seeder = new DbSeeder(dataContext);
        await seeder.SeedAsync().ConfigureAwait(false);
    }

    [Test]
    public async Task GetTest()
    {
        IEnumerable<Employee> entities = await employeeService.GetAsync().ConfigureAwait(false);
        
        Assert.IsNotEmpty(entities);
        
        Assert.DoesNotThrowAsync(async() => await employeeService.GetByIdAsync(entities.First().Id).ConfigureAwait(false));
        Assert.DoesNotThrowAsync(async() => await employeeService.GetByEmailAsync(entities.First().Email).ConfigureAwait(false));

        Assert.ThrowsAsync<EmployeeNotFoundException>(async () => await employeeService.GetByEmailAsync(string.Empty).ConfigureAwait(false));
        Assert.ThrowsAsync<EmployeeNotFoundException>(async () => await employeeService.GetByIdAsync(Guid.NewGuid()).ConfigureAwait(false));
    }
    
    [Test]
    public async Task AddTest()
    {
        var entity = new Employee()
        {
            Name = "Joe Smith",
            Email = "joe.smith@intracto.com",
            HomeLocation = new Point(4.393744, 50.7972951),
            DefaultWorkLocation = new Point(4.4075614900783595, 51.20901005),
            DefaultCommuteType = CommuteType.Car
        };

        Assert.AreEqual(Guid.Empty, entity.Id);
        
        Employee addedEntity = await employeeService.AddAsync(entity).ConfigureAwait(false);
        
        Assert.AreNotEqual(Guid.Empty, addedEntity.Id);
        Assert.DoesNotThrowAsync(async () => await employeeService.GetByEmailAsync(entity.Email).ConfigureAwait(false));
        Assert.DoesNotThrowAsync(async () => await employeeService.GetByIdAsync(entity.Id).ConfigureAwait(false));
    }
    
    [Test]
    public async Task UpdateTest()
    {
        IEnumerable<Employee> entities = await employeeService.GetAsync().ConfigureAwait(false);
        
        Assert.IsNotEmpty(entities);

        Employee entityToUpdate = entities.First(x => x.DefaultCommuteType != CommuteType.Train);

        var updatedAtBeforeUpdate = entityToUpdate.UpdatedAt;
        var commuteTypeBeforeUpdate = entityToUpdate.DefaultCommuteType;
        
        entityToUpdate.DefaultCommuteType = CommuteType.Train;

        Employee updatedEntity = await employeeService.UpdateAsync(entityToUpdate).ConfigureAwait(false);

        entities = await employeeService.GetAsync().ConfigureAwait(false);
        
        Assert.AreNotEqual(updatedAtBeforeUpdate, entities.First(x => x.Id.Equals(entityToUpdate.Id)).UpdatedAt);
        Assert.AreNotEqual(CommuteType.Train, commuteTypeBeforeUpdate);
        Assert.AreEqual(CommuteType.Train, entities.First(x => x.Id.Equals(entityToUpdate.Id)).DefaultCommuteType);
    }

    [Test]
    public async Task DeleteTest()
    {
        IEnumerable<Employee> entities = await employeeService.GetAsync().ConfigureAwait(false);
        
        Assert.IsNotEmpty(entities);
        
        Employee entityToDelete = entities.Last();

        bool result = await employeeService.DeleteAsync(entityToDelete.Id).ConfigureAwait(false);
        
        Assert.IsTrue(result);

        entities = await employeeService.GetAsync().ConfigureAwait(false);
        
        Assert.IsFalse(entities.Any(x => x.Equals(entityToDelete)));
    }
}