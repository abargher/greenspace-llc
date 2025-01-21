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
    public bool IsPowerpoint {get; set;}
    public int IndexInQueue {get; set;}
    public EmailEntry entry {get; set;}

    public Email(bool isTask, bool isPowerpoint, string filepath){
        IsTask = isTask;
        IsPowerpoint = isPowerpoint;
        Filepath  = filepath;
        this.ParseEmailFromFilepath();

    }
    private void ParseEmailFromFilepath()
    {
         // line 1 Subject line
         // line 2 sender
         // line 3 newline
         // line 4-EOF body
        GD.Print("Parsing email from filepath: ", Filepath);

        var lines = File.ReadLines(Filepath);
        string subjectLine = lines.First();
        string sender = lines.Skip(1).First();
        string body = "";
        foreach (string line in lines.Skip(2))
        {
            body += (line + "\n");
        }

        SubjectLine = subjectLine;
        Sender = sender;
        BodyText = body;
    }
    public EmailEntry ToEmailEntry()
    {
        EmailEntry result = GD.Load<PackedScene>("res://scenes/email_entry.tscn").Instantiate<EmailEntry>();
        RichTextLabel senderLine = (RichTextLabel)result.GetNode("EmailEntryButton/SenderLine");
        RichTextLabel subjectLine = (RichTextLabel)result.GetNode("EmailEntryButton/SubjectLine");
        RichTextLabel body = (RichTextLabel)result.GetNode("EmailEntryButton/BodyLine");

        senderLine.Text = "From: " + Sender;
        subjectLine.Text = SubjectLine;
        body.Text = BodyText;

        entry = result;

        return result;
    }
}
public partial class OfficePcView : Control
{
	SceneManager sceneManager;
    public static MetricsHud metricsHud { get; private set; }
    private Gameplay gameplay { get; set;}
    // queue of emails to be displayed.
    public Email[] EmailQueue { get; set; }
	public override void _Ready()
	{
        GD.Print("OfficePcView Loaded In, _Ready called");
		sceneManager = GetNode<SceneManager>("/root/SceneManager");

        metricsHud = GetNode<MetricsHud>("/root/Gameplay/HUDManager/MetricsHUD");
		metricsHud.CallDeferred(nameof(MetricsHud.OnChangeSEO), 10, 5, 2);
        gameplay = GetNode<Gameplay>("/root/Gameplay");


        int currDay = gameplay.currentDay;
        bool hasDoneWaterCooler = gameplay.hasDoneWaterCooler;
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

        gameplay.backgroundPlayer.Stop();
        gameplay.backgroundPlayer.Stream = gameplay.officeSounds[Math.Min(gameplay.currentDay - 1, gameplay.officeSounds.Count - 1)];
        gameplay.backgroundPlayer.Play();

        int currDay = gameplay.currentDay;
        bool hasDoneWaterCooler = gameplay.hasDoneWaterCooler;
        GD.Print("===== NEW Office Scene Day: ", currDay);

        AssignTasksAndLoadEmails(currDay,hasDoneWaterCooler);

    }

    public void AssignTasksAndLoadEmails(int currDay, bool hasDoneWaterCooler)
    {
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
                GD.Print("ASSIGNING TASKS");
                gameplay.dailyPowerpointsRemaining = 2;
                gameplay.dailyGreenliningPapersRemaining = 0;
                gameplay.dailyFluffEmailsRemaining = 3;
                Email email02 = new Email(isTask: true,
                                          isPowerpoint: true,
                                          filepath: "assets/text/emails/day01/pre-cooler/has-reply/email-02.txt");
                Email email03 = new Email(isTask: true,
                                          isPowerpoint: true,
                                          filepath: "assets/text/emails/day01/pre-cooler/has-reply/email-03.txt");

                Email email00 = new Email(isTask: false,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day01/pre-cooler/no-reply/email-00.txt");
                Email email01 = new Email(isTask: false,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day01/pre-cooler/no-reply/email-01.txt");
                Email email04 = new Email(isTask: false,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day01/pre-cooler/no-reply/email-04.txt");

                Email[] dayOneEmails = new Email[5]{email02,email03,email00,email01,email04};
                EmailQueue = dayOneEmails;
                EmailApp.Instance.EmitSignal(EmailApp.SignalName.EmailsLoaded);
            } else {
                // no tasks
                return;
            }
        } else if (currDay == 2) {
            if (!hasDoneWaterCooler) {
                gameplay.dailyPowerpointsRemaining = 2;
                gameplay.dailyGreenliningPapersRemaining = 0;
                gameplay.dailyFluffEmailsRemaining = 2;
                Email email06 = new Email(isTask: true,
                                          isPowerpoint: true,
                                          filepath: "assets/text/emails/day02/pre-cooler/has-reply/email-06.txt");
                Email email07 = new Email(isTask: true,
                                          isPowerpoint: true,
                                          filepath: "assets/text/emails/day02/pre-cooler/has-reply/email-07.txt");


                Email email05 = new Email(isTask: false,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day02/pre-cooler/no-reply/email-05.txt");
                Email email08 = new Email(isTask: false,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day02/pre-cooler/no-reply/email-08.txt");
                Email[] emails = new Email[4]{email06,email07,email05,email08};
                EmailQueue = emails;
                EmailApp.Instance.EmitSignal(EmailApp.SignalName.EmailsLoaded);
            } else { //post water cooler
                // no tasks
            }
        } else if (currDay == 3) {
            if (!hasDoneWaterCooler) {
                gameplay.dailyPowerpointsRemaining = 1;
                gameplay.dailyGreenliningPapersRemaining = 1;
                gameplay.dailyFluffEmailsRemaining = 2;
                Email email10 = new Email(isTask: true,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day03/pre-cooler/has-reply/email-10.txt");
                Email email11 = new Email(isTask: true,
                                          isPowerpoint: true,
                                          filepath: "assets/text/emails/day03/pre-cooler/has-reply/email-11.txt");
            
            
                Email email09 = new Email(isTask: false,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day03/pre-cooler/no-reply/email-09.txt");
                Email email12 = new Email(isTask: false,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day03/pre-cooler/no-reply/email-12.txt");
                Email[] emails = new Email[4]{email10,email11,email09,email12};
                EmailQueue = emails;
                EmailApp.Instance.EmitSignal(EmailApp.SignalName.EmailsLoaded);
            } else { //post water cooler
                gameplay.dailyPowerpointsRemaining = 0;
                gameplay.dailyGreenliningPapersRemaining = 0;
                gameplay.dailyFluffEmailsRemaining = 1;
                Email email13 = new Email(isTask: false,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day03/post-cooler/no-reply/email-13.txt");
                Email[] emails = new Email[1]{email13};
                EmailQueue = emails;
                EmailApp.Instance.EmitSignal(EmailApp.SignalName.EmailsLoaded);
            }
        } else if (currDay == 4) {
            if (!hasDoneWaterCooler) {
                gameplay.dailyPowerpointsRemaining = 1;
                gameplay.dailyGreenliningPapersRemaining = 0;
                gameplay.dailyFluffEmailsRemaining = 1;
                Email email14 = new Email(isTask: true,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day04/pre-cooler/has-reply/email-14.txt");

                Email email15 = new Email(isTask: false,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day04/pre-cooler/no-reply/email-15.txt");
                Email[] emails = new Email[2]{email14,email15};
                EmailQueue = emails;
                EmailApp.Instance.EmitSignal(EmailApp.SignalName.EmailsLoaded);
            } else { //post water cooler
                gameplay.dailyPowerpointsRemaining = 1;
                gameplay.dailyGreenliningPapersRemaining = 0;
                gameplay.dailyFluffEmailsRemaining = 0;
                Email email16 = new Email(isTask: true,
                                          isPowerpoint: true,
                                          filepath: "assets/text/emails/day04/post-cooler/has-reply/email-16.txt");
                Email[] emails = new Email[1]{email16};
                EmailQueue = emails;
                EmailApp.Instance.EmitSignal(EmailApp.SignalName.EmailsLoaded);
            }
        } else if (currDay == 5) {
            if (!hasDoneWaterCooler) {
                gameplay.dailyPowerpointsRemaining = 1;
                gameplay.dailyGreenliningPapersRemaining = 1;
                gameplay.dailyFluffEmailsRemaining = 2;
                Email email18 = new Email(isTask: true,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day05/pre-cooler/has-reply/email-18.txt");
                Email email20 = new Email(isTask: true,
                                          isPowerpoint: true,
                                          filepath: "assets/text/emails/day05/pre-cooler/has-reply/email-20.txt");

                Email email17 = new Email(isTask: false,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day05/pre-cooler/no-reply/email-17.txt");
                Email email19 = new Email(isTask: false,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day05/pre-cooler/no-reply/email-19.txt");
                Email[] emails = new Email[4]{email18,email20,email17,email19};
                EmailQueue = emails;
                EmailApp.Instance.EmitSignal(EmailApp.SignalName.EmailsLoaded);

            } else { //post water cooler
            }
        } else if (currDay == 6) {
            if (!hasDoneWaterCooler) {
                gameplay.dailyPowerpointsRemaining = 1;
                gameplay.dailyGreenliningPapersRemaining = 1;
                gameplay.dailyFluffEmailsRemaining = 2;
                Email email22 = new Email(isTask: true,
                                          isPowerpoint: true,
                                          filepath: "assets/text/emails/day06/pre-cooler/has-reply/email-22.txt");
                Email email23 = new Email(isTask: true,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day06/pre-cooler/has-reply/email-23.txt");


                Email email21 = new Email(isTask: false,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day06/pre-cooler/no-reply/email-21.txt");
                Email email24 = new Email(isTask: false,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day06/pre-cooler/no-reply/email-24.txt");
                Email[] emails = new Email[4]{email22,email23,email21,email24};
                EmailQueue = emails;
                EmailApp.Instance.EmitSignal(EmailApp.SignalName.EmailsLoaded);
            } else { //post water cooler
                gameplay.dailyPowerpointsRemaining = 0;
                gameplay.dailyGreenliningPapersRemaining = 0;
                gameplay.dailyFluffEmailsRemaining = 1;
                Email email25 = new Email(isTask: false,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day06/post-cooler/no-reply/email-25.txt");
                Email[] emails = new Email[1]{email25};
                EmailQueue = emails;
                EmailApp.Instance.EmitSignal(EmailApp.SignalName.EmailsLoaded);
            }
        } else if (currDay == 7) {
            if (!hasDoneWaterCooler) {
                gameplay.dailyPowerpointsRemaining = 0;
                gameplay.dailyGreenliningPapersRemaining = 1;
                gameplay.dailyFluffEmailsRemaining = 2;
                Email email26 = new Email(isTask: false,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day07/pre-cooler/no-reply/email-26.txt");
                Email email27 = new Email(isTask: false,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day07/pre-cooler/no-reply/email-27.txt");
                Email day7greenline = new Email(isTask: true,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day07/pre-cooler/has-reply/day7email.txt");
                Email[] emails = new Email[3]{email26,email27,day7greenline};
                EmailQueue = emails;
                EmailApp.Instance.EmitSignal(EmailApp.SignalName.EmailsLoaded);
            } else { //post water cooler
            }
        } else if (currDay == 8) {
            if (!hasDoneWaterCooler) {
                gameplay.dailyPowerpointsRemaining = 0;
                gameplay.dailyGreenliningPapersRemaining = 1;
                gameplay.dailyFluffEmailsRemaining = 2;
                Email email28 = new Email(isTask: false,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day08/pre-cooler/no-reply/email-28.txt");
                Email email29 = new Email(isTask: false,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day08/pre-cooler/no-reply/email-29.txt");
                
                Email day8greenline = new Email(isTask: true,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day08/pre-cooler/has-reply/day8email.txt");
                Email[] emails = new Email[3]{email28,email29,day8greenline};
                EmailQueue = emails;
                EmailApp.Instance.EmitSignal(EmailApp.SignalName.EmailsLoaded);
            } else { //post water cooler
            }
        } else if (currDay == 9) {
            if (!hasDoneWaterCooler) {
                gameplay.dailyPowerpointsRemaining = 0;
                gameplay.dailyGreenliningPapersRemaining = 1;
                gameplay.dailyFluffEmailsRemaining = 1;
                Email email30 = new Email(isTask: false,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day09/pre-cooler/no-reply/email-30.txt");
                Email day9greenline = new Email(isTask: true,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day09/pre-cooler/has-reply/day9email.txt");
                Email[] emails = new Email[2]{email30,day9greenline};
                EmailQueue = emails;
                EmailApp.Instance.EmitSignal(EmailApp.SignalName.EmailsLoaded);
            } else { //post water cooler
            }
        } else if (currDay == 10) {
            if (!hasDoneWaterCooler) {
                gameplay.dailyPowerpointsRemaining = 0;
                gameplay.dailyGreenliningPapersRemaining = 1;
                gameplay.dailyFluffEmailsRemaining = 1;
                Email email31 = new Email(isTask: false,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day10/pre-cooler/no-reply/email-31.txt");
                Email day10greenline = new Email(isTask: true,
                                          isPowerpoint: false,
                                          filepath: "assets/text/emails/day010/pre-cooler/has-reply/day10email.txt");
                Email[] emails = new Email[2]{email31,day10greenline};
                EmailQueue = emails;
                EmailApp.Instance.EmitSignal(EmailApp.SignalName.EmailsLoaded);
            } else { //post water cooler
            }
        }
        gameplay.IfDoneWithTasksSwapScene();

    }

	public void MoveLeft()
	{
		sceneManager.SwapScenes("res://scenes/left_view.tscn", GetNode<Gameplay>("/root/Gameplay"), this, "fade_to_black");
	}

	public void MoveRight()
	{
		sceneManager.SwapScenes("res://scenes/right_view.tscn", GetNode<Gameplay>("/root/Gameplay"), this, "fade_to_black");

	}

    public void OnTestButtonClick()
    {
        sceneManager.SwapScenes("res://scenes/water_cooler.tscn", GetNode<Gameplay>("/root/Gameplay"), this, "fade_to_black");

    }
}
