﻿namespace Shared
{
    public class ItemUpdatedEvent
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
