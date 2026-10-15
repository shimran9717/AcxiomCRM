namespace CRMPlatform.Web.Models.ViewModels;

public class DashboardViewModel
{
    // --- KPI Metrics ---
    public int TotalCustomers { get; set; }
    public int TotalLeads { get; set; }
    public int OpenLeads { get; set; }
    public int TotalOpportunities { get; set; }
    public int OpenOpportunities { get; set; }
    public int WonOpportunities { get; set; }
    public int LostOpportunities { get; set; }
    public decimal TotalPipelineValue { get; set; }
    public decimal WeightedPipelineValue { get; set; }

    // --- Chart 1: Lead Status Breakdown (Doughnut) ---
    public List<string> LeadStatusLabels { get; set; } = new();
    public List<int> LeadStatusCounts { get; set; } = new();

    // --- Chart 2: Opportunity Pipeline Funnel (Bar) ---
    public List<string> StageLabels { get; set; } = new();
    public List<decimal> StageValues { get; set; } = new();
    public List<int> StageCounts { get; set; } = new();

    // --- Chart 3: Monthly Closed/Won Trend (Line/Bar) ---
    public List<string> MonthlyLabels { get; set; } = new();
    public List<decimal> MonthlyWonRevenue { get; set; } = new();
}