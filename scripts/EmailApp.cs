using Godot;
using System;
using System.IO;

public partial class EmailApp : Control
{
    public Email currEmail { get; set; }
    [Export]
    public RichTextLabel SubmitTaskTooltip;
    public Gameplay gameplay;
    public OfficePcView officePcView;
	[Signal]
	public delegate void EmailAppCloseEventHandler();

	[Signal]
	public delegate void EmailAppReplyEventHandler();
    [Signal]
	public delegate void EmailsLoadedEventHandler();
    [Signal]
	public delegate void EmailSelectedEventHandler(int emailIndexInQueue);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        Button replyOrReadEmailButton = (Button)GetNode("HBoxContainer/MainPanel/EmailButtons/ReplyOrReadEmailButton");
        Button closeEmailButton = (Button)GetNode("HBoxContainer/MainPanel/EmailButtons/CloseEmailButton");

        gameplay = GetNode<Gameplay>("/root/Gameplay");
        officePcView = GetNode<OfficePcView>("/root/Gameplay/OfficePCView");
        officePcView.EmailsLoaded += OnEmailsLoaded;
        
        // if we have time to add button text
        // Color red = new Color(1,0,0,1);
        // replyOrReadEmailButton.Set("theme_override_colors/font_color",red);
        // closeEmailButton.Set("theme_override_colors/font_color",red);
	}

    public void OnTimerTimeout()
    {
        SubmitTaskTooltip.Visible = false;
    }

	public void OnEmailCloseButtonPressed()
	{
        DeselectEmail();
	}

	public void OnEmailReplyOrReadButtonPressed()
	{
		// GetChild<Button>(0).Icon = GD.Load<Texture2D>("res://icon_reply.png");
        if (currEmail == null) {
            GD.Print("email reply TO NOTHING pressed");
            return;
        }
        ResolveCurrEmail();
        return;
        if (currEmail.IsTask) {
            if (currEmail.IsPowerpoint) {
                if (gameplay.numPowerpointsCompleted > 0)  {
                    // submit one powerpoint
                    gameplay.numPowerpointsCompleted--;
                    gameplay.dailyPowerpointsRemaining--;
                    ResolveCurrEmail();
                } else if (gameplay.numPowerpointsCompleted <= 0) {
                    // right now the task is never finished
                    SubmitTaskTooltip.Visible = true;
                    Timer timer = (Timer)GetNode("Timer");
                    timer.Start();
                }
            } else {
                // is greenlining
                if (gameplay.numDocumentsInMailbox > 0) {
                    // submit one report
                    gameplay.numDocumentsInMailbox--;
                    gameplay.dailyGreenliningPapersRemaining--;
                    ResolveCurrEmail();

                } else if (gameplay.numDocumentsInMailbox <= 0) {
                    // right now the task is never finished
                    SubmitTaskTooltip.Visible = true;
                    Timer timer = (Timer)GetNode("Timer");
                    timer.Start();
                }
            }

        } else {
            // no conditions for marking a fluff email as read.
            gameplay.dailyFluffEmailsRemaining--;
            ResolveCurrEmail();
        }
	}
    public void ResolveCurrEmail(){
        // take the current email, be it task or fluff
        // and remove it from the inbox, because it's work has been completed. 

        // check if gameplay dailies are done
        GD.Print("resolving email: ", currEmail.SubjectLine);
        currEmail.isCompleted = true;
        currEmail.entry.Visible = false;
        DeselectEmail();
        gameplay.IncrementTimeOfDay(19);
        gameplay.IfDoneWithTasksSwapScene();

        
    }
    public void OnEmailsLoaded()
    {
        GD.Print("AAAAAA emails trying to be loaded");
        OfficePcView officeview = (OfficePcView)GetNode("/root/Gameplay/OfficePCView");
        int index = 0;
        foreach (Email email in gameplay.currentDayEmails)
        {
            email.IndexInQueue = index;
            EmailEntry entry = email.ToEmailEntry();
            if (email.isCompleted) {
                entry.Visible = false;
            }
            entry.IndexInQueue = index;
            VBoxContainer inboxleftcol = (VBoxContainer)GetNode("HBoxContainer/LeftPanel/Inbox/VBoxContainer");
            inboxleftcol.AddChild(entry);
            index++;
            entry.EmailSelected += OnEmailSelected;
        }

        foreach(Email email in gameplay.currentDayEmails)
        {
            if (!email.isCompleted) {
                SelectEmail(email);
                break;
            }
        }

    }
    public void DeselectEmail()
    {
        RichTextLabel subject = (RichTextLabel)GetNode("HBoxContainer/MainPanel/SubjectLine");
        RichTextLabel sender = (RichTextLabel)GetNode("HBoxContainer/MainPanel/Sender/SenderLine");
        RichTextLabel body = (RichTextLabel)GetNode("HBoxContainer/MainPanel/EmailBody");
        TextureRect senderImage = (TextureRect)GetNode("HBoxContainer/MainPanel/Sender/SenderIcon");

        subject.Text = "";
        sender.Text = "";
        body.Text = "";
        senderImage.Texture = null;

        currEmail = null;

    }
    public void SelectEmail(Email email)
    {
        // method to select an email, and make it 
        
        RichTextLabel subject = (RichTextLabel)GetNode("HBoxContainer/MainPanel/SubjectLine");
        RichTextLabel sender = (RichTextLabel)GetNode("HBoxContainer/MainPanel/Sender/SenderLine");
        RichTextLabel body = (RichTextLabel)GetNode("HBoxContainer/MainPanel/EmailBody");
        TextureRect senderImage = (TextureRect)GetNode("HBoxContainer/MainPanel/Sender/SenderIcon");

        Button replyOrReadEmailButton = (Button)GetNode("HBoxContainer/MainPanel/EmailButtons/ReplyOrReadEmailButton");

        subject.Text = "[font_size=36]" + email.SubjectLine + "[/font_size]";
        sender.Text = "[font_size=24]" + email.Sender + "[/font_size]";
        body.Text = "[font_size=36]" + email.BodyText + "[/font_size]";
        senderImage.Texture = GD.Load<Texture2D>("res://assets/images/sprites/green_space_logo_crop.png");

        if (email.IsTask) {
            // reply
            // replyOrReadEmailButton.Icon = GD.Load<Texture2D>("res://assets/images/sprites/reply_email.png");
            Theme replyTheme = GD.Load<Theme>("res://assets/themes/email_reply_theme.tres");
            replyOrReadEmailButton.Theme = replyTheme;
            // replyOrReadEmailButton.Text = "Reply and Submit Work";
        } else {
            // read
            // replyOrReadEmailButton.Icon = GD.Load<Texture2D>("res://assets/images/sprites/mark_read.png");
            // replyOrReadEmailButton.Text = "Mark Email as Read";
            Theme readTheme = GD.Load<Theme>("res://assets/themes/email_read_theme.tres");
            replyOrReadEmailButton.Theme= readTheme;
        }

        currEmail = email;

    }
    public void OnEmailSelected(Email emailContents)
    {
        // OfficePcView officeview = (OfficePcView)GetNode("/root/Gameplay/OfficePCView");
        // Email email = officeview.EmailQueue[emailIndexInQueue];
        SelectEmail(emailContents);
        
    }
}
