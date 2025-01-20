using Godot;
using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;

public class Email
{
    public static string Filepath {get; set;}
    public string SubjectLine { get; set; }
    public string Sender { get; set; }
    public string BodyText { get; set; }
    public bool IsTask {get; set;}

    public Email(bool isTask, string filepath){
        IsTask = isTask;
        Filepath  = filepath;
        this.ParseEmailFromFilepath();
    }
    private void ParseEmailFromFilepath()
    {
         // line 1 Subject line
         // line 2 sender
         // line 3 newline
         // line 4-EOF body

        var lines = File.ReadLines(Filepath);
        string subjectLine = lines.First();
        string sender = lines.First();
        string body = "";
        foreach (string line in lines)
        {
            body += lines;
        }
        GD.Print("Subject Line: ", subjectLine);
        GD.Print("Sender: ", sender);
        GD.Print("Body: ", body);
    }
}
public partial class OfficePcView : Control
{
	SceneManager sceneManager;
    public static MetricsHud metricsHud { get; private set; }
    private Gameplay Gameplay { get; set;}
	public override void _Ready()
	{
        GD.Print("OfficePcView Loaded In, _Ready called");
		sceneManager = GetNode<SceneManager>("/root/SceneManager");

        metricsHud = GetNode<MetricsHud>("/root/Gameplay/HUDManager/MetricsHUD");
		metricsHud.CallDeferred(nameof(MetricsHud.OnChangeSEO), 10, 5, 2);
        Gameplay = GetParent<Gameplay>();

        int currDay = Gameplay.currentDay;
        bool hasDoneWaterCooler = Gameplay.hasDoneWaterCooler;
        GD.Print("===== NEW Office Scene Day: ", currDay);

        AssignTasksAndLoadEmails(currDay,hasDoneWaterCooler);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    // when you come to work each day.
    public void InitScene()
    {
        // - InitScene(): Called after the scene has been added to the SceneTree; can initialize any values for the scene.
        int currDay = Gameplay.currentDay;
        bool hasDoneWaterCooler = Gameplay.hasDoneWaterCooler;
        GD.Print("===== NEW Office Scene Day: ", currDay);

        AssignTasksAndLoadEmails(currDay,hasDoneWaterCooler);

    }

    public void AssignTasksAndLoadEmails(int currDay, bool hasDoneWaterCooler)
    {
        // update internal task counters based on day's email contents
        /*
        1:before
        response
        email-02.txt powerpoint
        email-03.txt powerpoint
        nr
        email-00.txt winter welcome
        email-01.txt albert hello
        email-04.txt prince woraso

        1:after
        nothing

        2:before
        response
        email-06.txt powerpoint
        email-07.txt powerpoint

        email-05.txt operation green space
        email-08.txt albert

        2:after
        nothing

        3:before
        response
        email-10.txt greenlining
        email-11.txt pp

        email-09.txt massive sucess
        email-12.txt pills

        3:after
        nr
        email-13.txt gorilla pills

        */

        GD.Print("AssignTasksAndLoadEmails with: ", currDay, hasDoneWaterCooler);
        if (currDay == 1) {
            /*
            1:before
            response
            email-02.txt powerpoint
res://assets/text/emails/day01/pre-cooler/has-reply/email-02.txt
            email-03.txt powerpoint
res://assets/text/emails/day01/pre-cooler/has-reply/email-03.txt
            nr
            email-00.txt winter welcome
res://assets/text/emails/day01/pre-cooler/no-reply/email-00.txt
            email-01.txt albert hello
res://assets/text/emails/day01/pre-cooler/no-reply/email-01.txt
            email-04.txt prince woraso
res://assets/text/emails/day01/pre-cooler/has-reply/email-04.txt
            */
            if (!hasDoneWaterCooler) {
                //pre-water-cooler
                Gameplay.dailyPowerpointsRemaining = 2;
                Gameplay.dailyGreenliningPapersRemaining = 0;
                Email email02 = new Email(isTask: true,
                                          filepath: "res://assets/text/emails/day01/pre-cooler/has-reply/email-02.txt");
            } else {
                // no tasks
                return;
            }

        }


    }

	public void MoveLeft()
	{
		sceneManager.SwapScenes("res://scenes/left_view.tscn", GetNode<Gameplay>("/root/Gameplay"), this, "fade_to_black");
	}

	public void MoveRight()
	{
		sceneManager.SwapScenes("res://scenes/right_view.tscn", GetNode<Gameplay>("/root/Gameplay"), this, "fade_to_black");

	}
}
