using MyCommute.Domain.Models;

namespace MyCommute.Tests.IntegrationTests;

[TestFixture]
public class GeoCodeServiceTests
{
    private IGeoCodeService geoCodeService;
    
    [OneTimeSetUp]
    public void GlobalSetup()
    {
        geoCodeService = new GeoCodeService();
    }

    [Test]
    public async Task GetCoordinatesForAddressTest()
    {
        Address address = new ("Grotesteenweg","214", "2600","Antwerpen", "België");

        Point coordinates = await geoCodeService.GetCoordinatesForAddressAsync(address).ConfigureAwait(false);
        
        Assert.AreEqual(4.4224318731099341, coordinates.X);
        Assert.AreEqual(51.193298900000002, coordinates.Y);
    }
    
    [Test]
    public async Task GetAddressForCoordinatesTest()
    {
        var coordinates = new Point(4.4224318731099341, 51.193298900000002);
        
        var address = await geoCodeService.GetAddressForCoordinatesAsync(coordinates).ConfigureAwait(false);
        
        Assert.AreEqual("Grotesteenweg", address.Street);
        Assert.AreEqual("214", address.Number);
        Assert.AreEqual("2600", address.ZipCode);
        Assert.AreEqual("be", address.Country);
    }
}
