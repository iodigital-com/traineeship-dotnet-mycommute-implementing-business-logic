namespace MyCommute.Tests.IntegrationTests;

[TestFixture]
public class CommuteServiceTests
{
    private ICommuteService commuteService;
    private static readonly Fixture Fixture = new ();
    
    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        var dataContext = new TestDataContext();
        commuteService = new CommuteService(dataContext);
        
        // Seed database
        var seeder = new DbSeeder(dataContext);
        await seeder.SeedAsync().ConfigureAwait(false);
    }

    [Test]
    public async Task GetTest()
    {
        var commutes = await commuteService.GetAllAsync().ConfigureAwait(false);
        
        Assert.IsNotEmpty(commutes);
        
        Assert.DoesNotThrowAsync(async () => await commuteService.GetAsync(commutes.First().Id).ConfigureAwait(false));
        
        var employeeCommutes = await commuteService.GetByUserIdAsync(commutes.First().Employee.Id).ConfigureAwait(false);
        
        Assert.IsNotEmpty(employeeCommutes);
        
        Assert.ThrowsAsync<CommuteNotFoundException>(async () => await commuteService.GetAsync(Guid.NewGuid()).ConfigureAwait(false));
        Assert.ThrowsAsync<CommuteNotFoundException>(async () => await commuteService.GetByUserIdAsync(Guid.NewGuid()).ConfigureAwait(false));
    }
    
    [Test]
    public async Task AddTest()
    {
        IEnumerable<Commute> entities = await commuteService.GetAllAsync().ConfigureAwait(false);

        Commute entity = new()
        {
            Employee = entities.Last().Employee,
            Type = CommuteType.Bike
        };

        Assert.AreEqual(Guid.Empty, entity.Id);
        
        Commute addedCommute = await commuteService.AddAsync(entity).ConfigureAwait(false);
        
        Assert.AreNotEqual(Guid.Empty, addedCommute.Id);
        Assert.DoesNotThrowAsync(async () => await commuteService.GetAsync(entity.Id).ConfigureAwait(false));
    }
    
    [Test]
    public async Task UpdateTest()
    {
        IEnumerable<Commute> entities = await commuteService.GetAllAsync().ConfigureAwait(false);
        
        Assert.IsNotEmpty(entities);

        Commute entityToUpdate = entities.First(x => x.Type != CommuteType.Car);

        var updatedAtBeforeUpdate = entityToUpdate.UpdatedAt;
        var commuteTypeBeforeUpdate = entityToUpdate.Type;
        
        entityToUpdate.Type = CommuteType.Car;

        Commute updatedEntity = await commuteService.UpdateAsync(entityToUpdate).ConfigureAwait(false);
        
        Assert.AreNotEqual(updatedAtBeforeUpdate, entities.First(x => x.Id.Equals(entityToUpdate.Id)).UpdatedAt);
        Assert.AreNotEqual(CommuteType.Car, commuteTypeBeforeUpdate);
        Assert.AreEqual(CommuteType.Car, entities.First(x => x.Id.Equals(entityToUpdate.Id)).Type);
    }

    [Test]
    public async Task DeleteTest()
    {
        IEnumerable<Commute> entities = await commuteService.GetAllAsync().ConfigureAwait(false);

        Commute entityToDelete = entities.First();

        bool result = await commuteService.DeleteAsync(entityToDelete.Id).ConfigureAwait(false);
        
        Assert.IsTrue(result);
        
        entities = await commuteService.GetAllAsync().ConfigureAwait(false);
        
        Assert.IsFalse(entities.Any(x => x.Equals(entityToDelete)));
    }
}