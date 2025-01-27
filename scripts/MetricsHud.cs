using Godot;
using System;

public partial class MetricsHud : ColorRect
{
    Gameplay gameplay;
    // all 5 RISEO percentage scores
    public int Synergy = 0;
    public int Efficiency = 0;
    public int Optimization = 0;
    public int RiskManagement = 0;
    public int Innovation = 0;
    public ProgressBar synergyProgressBar { get; set; }
    public ProgressBar efficiencyProgressBar { get; set; }
    public ProgressBar optimizationProgressBar { get; set; }
    public ProgressBar riskManagementProgressBar { get; set; }
    public ProgressBar innovationProgressBar { get; set; }


    [Signal]
    public delegate void ChangeOptimizationEventHandler(int pointsChange);
    [Signal]
    public delegate void ChangeSynergyEventHandler(int pointsChange);
    [Signal]
    public delegate void ChangeEfficiencyEventHandler(int pointsChange);
    [Signal]
    public delegate void ChangeRiskManagementEventHandler(int pointsChange);
    [Signal]
    public delegate void ChangeInnovationEventHandler(int pointsChange);

	public override void _Ready()
	{
        synergyProgressBar = GetNode<ProgressBar>("HBoxContainer/SynergyMeter/SynergyProgressBar");
        efficiencyProgressBar  = GetNode<ProgressBar>("HBoxContainer/EfficiencyMeter/EfficiencyProgressBar");
        optimizationProgressBar = GetNode<ProgressBar>("HBoxContainer/OptimizationMeter/OptimizationProgessBar");
        riskManagementProgressBar = GetNode<ProgressBar>("HBoxContainer/RiskManagementMeter/RiskManagementProgressBar");
        innovationProgressBar = GetNode<ProgressBar>("HBoxContainer/InnovationMeter/InnovationProgressBar");
        GD.Print(Synergy,Efficiency,Optimization);
        // DO NOT DELETE THIS PRINT
        GD.Print(synergyProgressBar,
                 efficiencyProgressBar,
                 optimizationProgressBar);
        AlignProgressBars();
	}


    // RISEO CHANGE SIGNALS
    public void AlignProgressBars()
    {
        efficiencyProgressBar.Value = Efficiency;
        synergyProgressBar.Value = Synergy;
        optimizationProgressBar.Value = Optimization;
        riskManagementProgressBar.Value = RiskManagement;
        innovationProgressBar.Value = Innovation;
    }

    public void ResetAllMetrics()
    {
        Synergy = 0;
        Efficiency = 0;
        Optimization = 0;
        RiskManagement = 0;
        Innovation = 0;

        synergyProgressBar.Value = Synergy;
        efficiencyProgressBar.Value = Efficiency;
        optimizationProgressBar.Value = Optimization;
        riskManagementProgressBar.Value = RiskManagement;
        innovationProgressBar.Value = Innovation;
    }
    public void OnChangeSEO(int synergyPointsChange, int efficiencyPointsChange, int optimizationPointsChange) 
    {
        OnChangeSynergy(synergyPointsChange);
        OnChangeEfficiency(efficiencyPointsChange);
        OnChangeOptimization(optimizationPointsChange);
    }
    public void OnChangeSynergy(int pointsChange)
    {
        Synergy = ComputeNewScore(pointsChange,Synergy);
        synergyProgressBar.Value = Synergy;
    }
    public void OnChangeEfficiency(int pointsChange)
    {
        Efficiency = ComputeNewScore(pointsChange,Efficiency);
        efficiencyProgressBar.Value = Efficiency;
    }

    public void OnChangeOptimization(int pointsChange)
    {
        Optimization = ComputeNewScore(pointsChange,Optimization);
        optimizationProgressBar.Value = Optimization;
    }
    public int ComputeNewScore(int pointsChange, int currMetricScore) 
    {
        // if the change fails, just keep the previous score
        int newMetricScore = currMetricScore;

        // function to take in the old value for some RISEO metric
        // and it's proposed change
        // and return the correct new value.
        if (currMetricScore < 0) 
        {
            GD.Print("metric was below 0 with: ", currMetricScore);
        }

        int distanceFromMax = 100 - currMetricScore;
        int distanceFromMin = currMetricScore;


        if (pointsChange > 0) {
            if (pointsChange <= distanceFromMax) {
                newMetricScore = currMetricScore + pointsChange;
                GD.Print("changing metric by:" + pointsChange);
            } else {
                // max out
                newMetricScore = 100;
                GD.Print("changing metric to:" + 100);
            }
        } else if (pointsChange <= 0) {
            if (pointsChange <= distanceFromMin) {

                newMetricScore = currMetricScore + pointsChange;
                GD.Print("changing metric by:" + pointsChange);
            } else {
                // bottom out
                newMetricScore = 0;
                GD.Print("changing metric to:" + 0);
            }
        }

        return newMetricScore;
    }
}
