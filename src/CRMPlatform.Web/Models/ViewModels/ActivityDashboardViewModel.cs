using CRMPlatform.Domain.Entities;

namespace CRMPlatform.Web.Models.ViewModels;

public class ActivityDashboardViewModel
{
    public IEnumerable<FollowUp> OverdueFollowUps { get; set; } = new List<FollowUp>();
    public IEnumerable<FollowUp> UpcomingFollowUps { get; set; } = new List<FollowUp>();
    public IEnumerable<Activity> RecentActivities { get; set; } = new List<Activity>();
}