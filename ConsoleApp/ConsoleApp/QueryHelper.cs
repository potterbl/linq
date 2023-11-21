using ConsoleApp.Model;
using ConsoleApp.Model.Enum;
using ConsoleApp.OutputTypes;

namespace ConsoleApp;

public class QueryHelper : IQueryHelper
{
    /// <summary>
    /// Get Deliveries that has payed
    /// </summary>
    //TODO: Завдання 1

    public IEnumerable<Delivery> Paid(IEnumerable<Delivery> deliveries) =>
        deliveries.Where(d => !string.IsNullOrEmpty(d.PaymentId));

    /// <summary>
    /// Get Deliveries that now processing by system (not Canceled or Done)
    /// </summary>
    //TODO: Завдання 2
    public IEnumerable<Delivery> NotFinished(IEnumerable<Delivery> deliveries) => deliveries.Where(d => 
        d.Status != DeliveryStatus.Cancelled && d.Status != DeliveryStatus.Done);

    /// <summary>
    /// Get DeliveriesShortInfo from deliveries of specified client
    /// </summary>
    //TODO: Завдання 3
    public IEnumerable<DeliveryShortInfo> DeliveryInfosByClient(IEnumerable<Delivery> deliveries, string clientId) =>
        deliveries
            .Where(d => d.ClientId == clientId)
            .Select(d => new DeliveryShortInfo
            {
                Id = d.Id,
                StartCity = d.Direction.Origin.City,
                EndCity = d.Direction.Destination.City,
                ClientId = d.ClientId,
                Type = d.Type,
                LoadingPeriod = d.LoadingPeriod,
                ArrivalPeriod = d.ArrivalPeriod,
                Status = d.Status,
                CargoType = d.CargoType
            });


    /// <summary>
    /// Get first ten Deliveries that starts at specified city and have specified type
    /// </summary>
    //TODO: Завдання 4
    public IEnumerable<Delivery> DeliveriesByCityAndType(IEnumerable<Delivery> deliveries, string cityName,
        DeliveryType type) =>
        deliveries
            .Where(d => d.Direction.Origin.City == cityName && d.Type == type);

    /// <summary>
    /// Order deliveries by status, then by start of loading period
    /// </summary>
    //TODO: Завдання 5
    public IEnumerable<Delivery> OrderByStatusThenByStartLoading(IEnumerable<Delivery> deliveries) =>
        deliveries
            .OrderBy(d => d.Status)
            .ThenBy(d => d.LoadingPeriod.Start);

    /// <summary>
    /// Count unique cargo types
    /// </summary>
    //TODO: Завдання 6
    public int CountUniqCargoTypes(IEnumerable<Delivery> deliveries) =>
        deliveries.Select(d => d.CargoType).Distinct().Count();
    
    /// <summary>
    /// Group deliveries by status and count deliveries in each group
    /// </summary>
    //TODO: Завдання 7
    public Dictionary<DeliveryStatus, int> CountsByDeliveryStatus(IEnumerable<Delivery> deliveries) =>
        deliveries.GroupBy(d => d.Status)
            .ToDictionary(group => group.Key, group => group.Count());

    /// <summary>
    /// Group deliveries by start-end city pairs and calculate average gap between end of loading period and start of arrival period (calculate in minutes)
    /// </summary>
    // TODO: Завдання 8
    public IEnumerable<AverageGapsInfo> AverageTravelTimePerDirection(IEnumerable<Delivery> deliveries) =>
        deliveries.GroupBy(d => new { StartCity = d.Direction.Origin.City, EndCity = d.Direction.Destination.City })
            .Select(group => new AverageGapsInfo
            {
                StartCity = group.Key.StartCity,
                EndCity = group.Key.EndCity,
                AverageGap = group.Average(delivery => (delivery.ArrivalPeriod.Start.Value - delivery.LoadingPeriod.End.Value).Minutes)
            });


    /// <summary>
    /// Paging helper
    /// </summary>
    //TODO: Завдання 9
    public IEnumerable<TElement> Paging<TElement, TOrderingKey>(IEnumerable<TElement> elements,
        Func<TElement, TOrderingKey> ordering,
        Func<TElement, bool>? filter = null,
        int countOnPage = 100,
        int pageNumber = 1) =>
        elements
            .Where(filter ?? (e => true))
            .OrderBy(ordering)
            .Skip((pageNumber - 1) * countOnPage)
            .Take(countOnPage);
}