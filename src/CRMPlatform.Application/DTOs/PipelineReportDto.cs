namespace CRMPlatform.Application.DTOs;

public record PipelineStageSummaryDto(
    string Stage,
    int TotalDeals,
    decimal TotalValue,
    decimal WeightedValue
);

public record PipelineReportResponseDto(
    decimal TotalPipelineValue,
    decimal TotalWeightedValue,
    int TotalActiveDeals,
    IEnumerable<PipelineStageSummaryDto> Stages
);