using Xunit;

public class CrudServiceAsyncTests
{
    [Fact]
    public async Task CreateAsync_ShouldAddProduct()
    {
        var service = new CrudServiceAsync<Product>(p => p.Id, "test-products.json");
        var product = Product.CreateNew();

        var added = await service.CreateAsync(product);
        var read = await service.ReadAsync(product.Id);

        Assert.True(added);
        Assert.Equal(product.Name, read?.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldChangeProduct()
    {
        var service = new CrudServiceAsync<Product>(p => p.Id, "test-products.json");
        var product = Product.CreateNew();
        await service.CreateAsync(product);

        product.Price = 999.99m;
        var updated = await service.UpdateAsync(product);
        var read = await service.ReadAsync(product.Id);

        Assert.True(updated);
        Assert.Equal(999.99m, read?.Price);
    }

    [Fact]
    public async Task RemoveAsync_ShouldDeleteProduct()
    {
        var service = new CrudServiceAsync<Product>(p => p.Id, "test-products.json");
        var product = Product.CreateNew();
        await service.CreateAsync(product);

        var removed = await service.RemoveAsync(product);
        var read = await service.ReadAsync(product.Id);

        Assert.True(removed);
        Assert.Null(read);
    }

    [Fact]
    public async Task ReadAllAsync_ShouldReturnPagedResults()
    {
        var service = new CrudServiceAsync<Product>(p => p.Id, "test-products.json");
        for (int i = 0; i < 50; i++)
            await service.CreateAsync(Product.CreateNew());

        var page = await service.ReadAllAsync(2, 10); // 2-а сторінка по 10
        Assert.Equal(10, page.Count());
    }
}
