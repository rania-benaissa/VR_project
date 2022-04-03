using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DebugController : MonoBehaviour
{
    Vector2 scroll = Vector2.zero;

    bool showConsole = false;

    bool showHelp = false;

    bool enteredText = false;

    string input;

    private List<DebugCommand<string[]>> commands;

    // COMMANDS
    public static DebugCommand<string[]> rotateCommand;
    public static DebugCommand<string[]> translateCommand;
    public static DebugCommand<string[]> scaleCommand;
    public static DebugCommand<string[]> planetRotationSunSpeedCommand;
    public static DebugCommand<string[]> planetRotationSpeedCommand;
    public static DebugCommand<string[]> HelpCommand;
    public static DebugCommand<string[]> HideHelpCommand;

    private void OnGUI()
    {
        // if the console is already drawn -> we don't dont do anything
        if (!showConsole)
        {

            Cursor.lockState = CursorLockMode.Locked;

            return;
        }

        //unlock the cursor besh we can write some code
        Cursor.lockState = CursorLockMode.None;

        // check si on clique sur entrée meme quand le focus est donné au text field
        enteredText = (Event.current.isKey && Event.current.keyCode == KeyCode.Return);

        float y = 10.0f;
        float boxHeight = 100;
        float x = 5.0f;
        float labelHeight = 50;


        GuiStyle();

        // si on demande a montrer le help de la console
        if (showHelp)
        {
            string[] labels = CreateLabels();

            // rectangle used inside scroll view
            Rect view = new Rect(x, y, Screen.width - labelHeight, labelHeight * labels.Length);


            GUI.Box(new Rect(x, y / 2, Screen.width - y, labelHeight * 3 + y / 2), "", GUI.skin.GetStyle("TextField"));

            // on show que 3 commandes à la fois
            Rect showedView = new Rect(x, y, Screen.width - y, labelHeight * 3 - y / 2);


            scroll = GUI.BeginScrollView(showedView, scroll, view);


            for (int i = 0; i < labels.Length; i++)
            {
                Rect rectLabel = new Rect(2 * x, labelHeight * i + (2 * y) / 2, view.width - 2 * x, labelHeight);
                GUI.Label(rectLabel, labels[i]);

            }

            GUI.EndScrollView();
            y += labelHeight * 3 + y / 2;


        }

        // On show la console si showConsole = true ( si showConsole = false on vient pas ici grace au return)
        GUI.Box(new Rect(0, y, Screen.width, boxHeight / 2), "");
        input = GUI.TextField(new Rect(x, y, Screen.width - 2 * x, boxHeight / 2), input);

    }

    private string[] CreateLabels()
    {
        string[] labels = new string[]{

            "GameObject.Find(<Object>).transform.Rotate(new Vector3 (<x>,<y>,<z>));",
            "GameObject.Find(<Object>).transform.Rotate(new Vector3(<x>,<y>,<z>) * speed);",
            "GameObject.Find(<Object>).transform.Translate(new Vector3(<x>,<y>,<z>) * speed);",
            "GameObject.Find(<Object>).transform.localScale *= scaleFactor;",
            "GameObject.Find(<Object>).GetComponent<RotateAround>().PlanetRotationSpeed = <value>;",
            "GameObject.Find(<Object>).GetComponent<RotateAround>().RotationSpeedAroundSun = <value>;",
            "help - shows documentation help",
            "hide - hides documentation help"

        };

        return labels;
    }

    //customize GUI
    private void GuiStyle()
    {
        // TEXT COLOR + all
        GUI.color = new Color(0.24f, 0.24f, 0.24f);

        //we change text size
        GUI.skin.textField.fontSize = 24;
        GUI.skin.label.fontSize = 24;
        // on met le texte verticalement au milieu
        GUI.skin.textField.alignment = TextAnchor.MiddleLeft;
    }

    private void Awake()
    {
        // HERE WE CREATE OUR COMMANDS AND ADD THEM TO OUR LIST

        rotateCommand = new DebugCommand<string[]>(".transform.Rotate", (vals) =>
         {
             string planet = vals[1];

             float x = float.Parse(vals[5].Replace(".", ","));
             float y = float.Parse(vals[6].Replace(".", ","));
             float z = float.Parse(vals[7].Replace(".", ","));

             GameObject.Find(planet).transform.Rotate(new Vector3(x, y, z));

             if (vals.Length > 8)
             {

                 float speed = float.Parse(vals[8].Replace(".", ","));

                 GameObject.Find(planet).transform.Rotate(new Vector3(x, y, z) * speed);
             }

         });

        translateCommand = new DebugCommand<string[]>(".transform.Translate", (vals) =>
        {
            string planet = vals[1];

            float x = float.Parse(vals[5].Replace(".", ","));
            float y = float.Parse(vals[6].Replace(".", ","));
            float z = float.Parse(vals[7].Replace(".", ","));


            GameObject.Find(planet).transform.Translate(new Vector3(x, y, z));

            if (vals.Length > 8)
            {

                float speed = float.Parse(vals[8].Replace(".", ","));

                GameObject.Find(planet).transform.Translate(new Vector3(x, y, z) * speed);
            }
        });

        scaleCommand = new DebugCommand<string[]>(".transform.localScale", (vals) =>
         {
             string planet = vals[1];
             float scaleFactor = float.Parse(vals[3].Replace(".", ","));

             GameObject.Find(planet).transform.localScale *= scaleFactor;

         });

        planetRotationSpeedCommand = new DebugCommand<string[]>(".GetComponent<RotateAround>().PlanetRotationSpeed", (vals) =>
         {
             float speed = float.Parse(vals[5].Replace(".", ","));
             string planet = vals[1];
             GameObject.Find(planet).GetComponent<RotateAround>().PlanetRotationSpeed = speed;

         });

        planetRotationSunSpeedCommand = new DebugCommand<string[]>(".GetComponent<RotateAround>().RotationSpeedAroundSun", (vals) =>
         {

             float speed = float.Parse(vals[5].Replace(".", ","));
             string planet = vals[1];

             GameObject.Find(planet).GetComponent<RotateAround>().RotationSpeedAroundSun = speed;

         });

        HelpCommand = new DebugCommand<string[]>("help", (vals) =>
        {
            showHelp = true;
        });

        HideHelpCommand = new DebugCommand<string[]>("hide", (vals) =>
        {
            showHelp = false;
        }

        );

        //OUR LIST
        commands = new List<DebugCommand<string[]>>
        {
            rotateCommand,translateCommand,scaleCommand,
            planetRotationSunSpeedCommand,planetRotationSpeedCommand,
            HelpCommand,HideHelpCommand

};

    }

    private void HandleInput()
    {
        // caracteres dequels je fais mon decoupage
        char[] delimiters = { '"', ')', '(', ',', '*', ' ', '<', '>', ';', '=' };


        string[] values = input.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < commands.Count; i++)
        {
            DebugCommandBase commandBase = commands[i] as DebugCommandBase;

            // si l id est le bon 
            if (input.Contains(commandBase.ID))
            {
                //+cast est le bon
                if (commands[i] as DebugCommand<string[]> != null)
                {
                    // cast back to the original form then invoke its command

                    (commands[i] as DebugCommand<string[]>).Invoke(values);

                }
            }
        }

        input = "";
    }

    private void Update()
    {

        // WHEN we press "TAB" we actually show/hide the console 

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            showConsole = !showConsole;
        }

        // si la console est activée et que je clique sur entrée 
        if (showConsole && enteredText)
        {
            // je controle la saisie du text et envoie la commande si y a un texte correct
            HandleInput();
        }

    }
}
