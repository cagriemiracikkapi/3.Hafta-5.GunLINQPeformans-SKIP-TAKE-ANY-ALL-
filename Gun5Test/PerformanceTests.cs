using System.Linq;
using NUnit.Framework;
using Week3.Gun5Lab;

[TestFixture]
public class PerformanceTests
{
    [Test]
    public void GetLogsByPage_SayfalamaMantik_DogruCalismali()
    {
        // Arrange
        var service = new LogService();
        int pageSize = 10;

        // Act - Sayfa 2'yi iste (11-20 arası olmalı, ama sıralama tersten olduğu için ID mantığı değişebilir)
        // Bizim mock datada ID 100 en yeni, ID 1 en eski.
        // Sayfa 1: 100..91
        // Sayfa 2: 90..81
        var page2 = service.GetLogsByPage(2, pageSize);

        // Assert
        Assert.That(page2.Count, Is.EqualTo(10));
        Assert.That(page2.First().Id, Is.EqualTo(11)); // 2. sayfanın başı
        Assert.That(page2.Last().Id, Is.EqualTo(20)); // 2. sayfanın sonu
    }
}
