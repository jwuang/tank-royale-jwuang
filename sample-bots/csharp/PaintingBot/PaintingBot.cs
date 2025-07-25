using System;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;
using Robocode.TankRoyale.BotApi.Graphics;

// ---------------------------------------------------------------------------
// PaintingBot
// ---------------------------------------------------------------------------
// A sample bot original made for Robocode by Pavel Savara based on MyFirstBot
//
// Demonstrates how to paint stuff on the battlefield.
// Remember to enable Graphical Debugging for the bot when running a battle.
// ---------------------------------------------------------------------------
public class PaintingBot : Bot
{
    float scannedX;
    float scannedY;
    int scannedTime;

    // The main method starts our bot
    static void Main(string[] args)
    {
        new PaintingBot().Start();
    }

    // Called when a new round is started -> initialize and do some movement
    public override void Run()
    {
        // Continuous forward and backward movement repeating forever
        while (IsRunning)
        {
            Forward(100);
            TurnGunLeft(360);
            Back(100);
            TurnGunLeft(360);
        }
    }

    // We saw another bot -> save the coordinates of the scanned bot and turn (time) when scanned
    public override void OnScannedBot(ScannedBotEvent e)
    {
        // Get the coordinates of the scanned bot and the time (turn) when scanned
        scannedX = (int)e.X;
        scannedY = (int)e.Y;
        scannedTime = e.TurnNumber;

        // Also, fire the gun!
        Fire(1);
    }

    // During each turn (tick), we draw a red circle at the bot's last known location. We can't draw
    // the circle at the bot's current position because we need to scan it again to determine its
    // updated location.
    public override void OnTick(TickEvent e)
    {
        // Check if we scanned a bot by checking if the scanned time is not 0
        if (scannedTime != 0)
        {
            // Calculate a color alpha value for transparency that.
            // The alpha value is at its maximum when a bot is initially scanned, gradually
            // diminishing over time as more time passes since the scan.
            int deltaTime = e.TurnNumber - scannedTime;
            uint alpha = (uint)Math.Max(0xff - (deltaTime * 16), 0);

            // Draw a red circle with the alpha value we calculated using anm ellipse
            var g = Graphics;

            var color = Color.FromRgba(0xff, 0x00, 0x00, alpha);
            g.SetFillColor(color);
            g.FillCircle(scannedX, scannedY, 20); // 20 is the radius of the bots bounding circle
        }
    }
}