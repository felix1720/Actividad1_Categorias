using Microsoft.EntityFrameworkCore;
using UT2024P4LP4.Web.Data;
using UT2024P4LP4.Web.Data.Dtos;
using UT2024P4LP4.Web.Data.Entities;

namespace UT2024P4LP4.Web.Services;

public interface ICategoriaService
{
    Task<ResultList<CategoriaDto>> GetAll(CancellationToken cancellationToken = default);


    Task<Result> Create(CategoriaRequest categoria);
    Task<Result> Delete(int Id);
    Task<Result> Update(CategoriaRequest categoria);
}

public class CategoriaService(IApplicationDbContext context) : ICategoriaService
{

    public async Task<ResultList<CategoriaDto>> GetAll(CancellationToken cancellationToken = default)
    {
        var categorias = await context.Categorias
        .Select(x => x.ToDto())
        .ToListAsync(cancellationToken);
        return ResultList<CategoriaDto>.Success(categorias);
    }

    //CRUD

    public async Task<Result> Create(CategoriaRequest categoria)
    {
        try
        {
            var entity = Categoria.Create(categoria.Nombre);
            context.Categorias.Add(entity);
            await context.SaveChangesAsync();
            return Result.Success("✅Category registered successfully!");
        }
        catch (Exception Ex)
        {
            return Result.Failure($"☠️ Error: {Ex.Message}");
        }
    }
    public async Task<Result> Update(CategoriaRequest categoria)
    {
        try
        {
            var entity = context.Categorias.Where(p => p.Id == categoria.Id).FirstOrDefault();
            if (entity == null)
                return Result.Failure($"The category '{categoria.Id}' does not exist!");
            if (entity.Update(categoria.Nombre))
            {
                await context.SaveChangesAsync();
                return Result.Success("✅Category updated successfully!");
            }
            return Result.Success("🐫 You have not done any change!");
        }
        catch (Exception Ex)
        {
            return Result.Failure($"☠️ Error: {Ex.Message}");
        }
    }
    public async Task<Result> Delete(int Id)
    {
        try
        {
            var entity = context.Categorias.Where(p => p.Id == Id).FirstOrDefault();
            if (entity == null)
                return Result.Failure($"The category '{Id}' does not exist!");
            context.Categorias.Remove(entity);
            await context.SaveChangesAsync();
            return Result.Success("✅Category removed successfully!");
        }
        catch (Exception Ex)
        {
            return Result.Failure($"☠️ Error: {Ex.Message}");
        }
    }
}
