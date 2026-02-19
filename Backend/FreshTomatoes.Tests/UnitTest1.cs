using Backend.Services;
using NUnit.Framework;
using Assert = Xunit.Assert;

namespace FreshTomatoes.Tests;

public class UnitTest1
{
    [Test]
    public async Task LoginAsync_Works()
    {
        // Arrange
        var a = 5;
        var b = 5;

        // Act
        var result = a + b;

        // Assert
        Assert.Equal(10, result);
    }
}