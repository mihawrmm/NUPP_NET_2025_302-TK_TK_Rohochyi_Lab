using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AsyncLab.Common;
using Xunit;

public class CrudServiceAsyncTests
{
    private readonly string _testFilePath = "test_buses.json";

    private CrudServiceAsync<Bus> CreateService()
    {
        if (File.Exists(_testFilePath))
            File.Delete(_testFilePath);

        return new CrudServiceAsync<Bus>(b => b.Id, _testFilePath);
    }

    [Fact]
    public async Task CreateAsync_AddsElementSuccessfully()
    {
        var service = CreateService();
        var bus = Bus.CreateNew();

        var result = await service.CreateAsync(bus);

        Assert.True(result);
        var read = await service.ReadAsync(bus.Id);
        Assert.Equal(bus.Id, read?.Id);
    }

    [Fact]
    public async Task ReadAsync_ReturnsCorrectElement()
    {
        var service = CreateService();
        var bus = Bus.CreateNew();
        await service.CreateAsync(bus);

        var result = await service.ReadAsync(bus.Id);

        Assert.NotNull(result);
        Assert.Equal(bus.Id, result!.Id);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesElementCorrectly()
    {
        var service = CreateService();
        var bus = Bus.CreateNew();
        await service.CreateAsync(bus);

        bus.Capacity = 999;
        await service.UpdateAsync(bus);

        var updated = await service.ReadAsync(bus.Id);
        Assert.Equal(999, updated?.Capacity);
    }

    [Fact]
    public async Task RemoveAsync_RemovesElementSuccessfully()
    {
        var service = CreateService();
        var bus = Bus.CreateNew();
        await service.CreateAsync(bus);

        var result = await service.RemoveAsync(bus);
        var readBack = await service.ReadAsync(bus.Id);

        Assert.True(result);
        Assert.Null(readBack);
    }

    [Fact]
    public async Task ReadAllAsync_ReturnsAllElements()
    {
        var service = CreateService();
        var buses = Enumerable.Range(0, 5).Select(_ => Bus.CreateNew()).ToList();
        foreach (var bus in buses)
            await service.CreateAsync(bus);

        var all = await service.ReadAllAsync();

        Assert.Equal(5, all.Count());
    }

    [Fact]
    public async Task ReadAllAsync_Paged_WorksCorrectly()
    {
        var service = CreateService();
        for (int i = 0; i < 10; i++)
            await service.CreateAsync(Bus.CreateNew());

        var page = await service.ReadAllAsync(page: 2, amount: 3);

        Assert.Equal(3, page.Count());
    }

    [Fact]
    public async Task SaveAsync_SerializesToFile()
    {
        var service = CreateService();
        var bus = Bus.CreateNew();
        await service.CreateAsync(bus);

        var result = await service.SaveAsync();

        Assert.True(result);
        Assert.True(File.Exists(_testFilePath));

        var content = await File.ReadAllTextAsync(_testFilePath);
        Assert.Contains(bus.Id.ToString(), content);
    }
}
