using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Data.Helpers;

public static class DbContextExtensions
{
    public static void SeedWork<T>(this Dsw2025TpiContext context, string dataSource) where T : class
    {
        if (context.Set<T>().Any()) return;

        var jsonPath = Path.Combine(AppContext.BaseDirectory, dataSource);
        var json = File.ReadAllText(jsonPath);

        try
        {
            var entities = JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            if (entities == null || entities.Count == 0)
            {
                Console.WriteLine($"No se encontraron entidades para insertar desde: {jsonPath}");
                return;
            }

            context.Set<T>().AddRange(entities);
            context.SaveChanges();
        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"Error al deserializar JSON desde: {jsonPath}");
            Console.WriteLine($"Mensaje: {jsonEx.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar los cambios: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Excepción interna: {ex.InnerException.Message}");
            }
            throw;
        }
    }

}
