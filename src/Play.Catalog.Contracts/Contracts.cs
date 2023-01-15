using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Catalog.Contracts
{
   public record CatalogItemCreated(Guid ItemId, String Name, String Description);
   public record CatalogItemUpdated(Guid ItemId, String Name, String Description);
   public record CatalogItemDeleted(Guid ItemId);
   
}