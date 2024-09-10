using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata;
using Server.Config;

namespace Server.Game.Spec
{
    public class SpecManager
    {
        public static SpecManager Instance { get; } = new SpecManager();
        private Dictionary<Type, IEntityType> _entityTypes = new Dictionary<Type, IEntityType>();
        private static Dictionary<Type, object> _specDictionary = new Dictionary<Type, object>();
        
        private readonly SpecDBContext _context = new SpecDBContext();

        public SpecManager()
        {
            _entityTypes.Add(typeof(SpecMap), _context.Model.FindEntityType("Server.Game.Spec.Tile"));
        }
        
        public void LoadAll()
        {
            foreach (var specType in _entityTypes.Keys)
            {
                var interfaceTypes = specType.GetInterfaces();

                foreach (var interfaceType in interfaceTypes)
                {
                    if (!interfaceType.IsGenericType) continue;
                    
                    var rawItemList = SelectItemList(specType);
                    
                    var constructor = specType.GetConstructor(new[] { typeof(List<>).MakeGenericType(typeof(object)) });
                    var spec = constructor.Invoke(new object[] {rawItemList});
                    _specDictionary.Add(specType, spec);
                    
                }
            }

            Console.WriteLine("게임 데이터 로드 완료");
        }

        public static T GetSpec<T>(Type specClass)
        {
            if (_specDictionary.TryGetValue(specClass, out var spec))
                return (T)spec;
            
            throw new ArgumentException($"[Spec] NOT FOUND SPEC!! {specClass}");
        }

        private List<object> SelectItemList(Type specType)
        {
            var entityType = _entityTypes[specType].ClrType;
            
            var method = _context.GetType().GetMethod("Set", new Type[] { }).MakeGenericMethod(entityType);
            var dbSet = method.Invoke(_context, null);
            
            var query = dbSet as IQueryable;

            var data = query.Cast<object>().ToList();
            Console.WriteLine($"Data Count: {data.Count}");

            return data;
        }
    }
}