using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GameClient.Characters;
using GameClient.Global.InputManager;
using GameClient.Renderable.Particle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using XNAnimation;

namespace GameClient.Global
{
    public static class RessourceProvider
    {
        public static Dictionary<string, Texture2D> CharacterFaces = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Texture2D> MenuBackgrounds = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Texture2D> Skyboxes = new Dictionary<string, Texture2D>();
        public static Dictionary<string, SkinnedModel> AnimatedModels = new Dictionary<string, SkinnedModel>();
        public static Dictionary<string, Model> StaticModels = new Dictionary<string, Model>();
        public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
        public static Dictionary<string, Texture2D> Cursors = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Dictionary<string, Texture2D>> Themes = new Dictionary<string, Dictionary<string, Texture2D>>();
        public static Dictionary<string, Video> Videos = new Dictionary<string, Video>();
        public static Dictionary<string, UniversalKeys> InputKeys = new Dictionary<string, UniversalKeys>();
        public static Dictionary<string, Dictionary<string, ParticleSystem>> Particles = new Dictionary<string, Dictionary<string, ParticleSystem>>();
        public static Dictionary<string, Character> Characters = new Dictionary<string, Character>();


        public static void Fill(ContentManager content, Game game)
        {
            LoadDictionnary(content, "CharacterFaces", CharacterFaces);
            LoadDictionnary(content, "MenuBackgrounds", MenuBackgrounds);
            LoadDictionnary(content, "Skyboxes", Skyboxes);
            LoadDictionnary(content, "StaticModels", StaticModels);
            LoadDictionnary(content, "Fonts", Fonts);
            LoadDictionnary(content, "Cursors", Cursors);
            LoadDictionnary(content, "Videos", Videos);

            FillParticles(content, game);
            LoadAnimatedModels(content);
            LoadThemes(content);
            LoadInputKeys();

            FillCharacters();
        }

        private static void LoadDictionnary<T>(ContentManager content, string dirName, Dictionary<string, T> dictionary)
        {
            foreach (var file in Directory.GetFiles("Content\\" + dirName).Select(Path.GetFileNameWithoutExtension).Where(file => !dictionary.ContainsKey(file)))
                dictionary.Add(file, content.Load<T>(dirName + '\\' + file));
        }

        #region AnimatedModels
        static void LoadAnimatedModels(ContentManager content)
        {
            AnimatedModels.Add("Zeus", GetZeus(content));
            AnimatedModels.Add("Spiderman", GetSpiderman(content));
            AnimatedModels.Add("Alex", GetAlex(content));
            AnimatedModels.Add("Ares", GetAres(content));
            AnimatedModels.Add("Ironman", GetIronman(content));
        }

        static SkinnedModel GetZeus(ContentManager content)
        {
            var sm = content.Load<SkinnedModel>("AnimatedModels\\Zeus\\Model");
            var body = content.Load<Texture2D>("AnimatedModels\\Zeus\\textura cor-2");
            var thunder = content.Load<Texture2D>("AnimatedModels\\Zeus\\Cube_001");

            foreach (ModelMesh mesh in sm.Model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.Texture = body;

                    if (mesh.Name == "Cube_001")
                        effect.Texture = thunder;

                    effect.EnableDefaultLighting();

                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;
                }
            }

            return sm;
        }
        static SkinnedModel GetIronman(ContentManager content)
        {
            var sm = content.Load<SkinnedModel>("AnimatedModels\\Ironman\\Model");
            var body = content.Load<Texture2D>("AnimatedModels\\Ironman\\texture");

            foreach (ModelMesh mesh in sm.Model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.Texture = body;

                    effect.EnableDefaultLighting();

                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;
                }
            }

            return sm;
        }
        static SkinnedModel GetSpiderman(ContentManager content)
        {
            var sm = content.Load<SkinnedModel>("AnimatedModels\\Spiderman\\Spiderman");
            var body = content.Load<Texture2D>("AnimatedModels\\Spiderman\\Spidermantex");

            foreach (ModelMesh mesh in sm.Model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.Texture = body;
                    effect.EnableDefaultLighting();

                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;
                }
            }

            return sm;
        }
        static SkinnedModel GetAlex(ContentManager content)
        {
            var sm = content.Load<SkinnedModel>("AnimatedModels\\Alex\\AlexMercer");
            var body = content.Load<Texture2D>("AnimatedModels\\Alex\\nis_alex_body_dm");
            var head = content.Load<Texture2D>("AnimatedModels\\Alex\\alex_head_shot_dm");
            var hand = content.Load<Texture2D>("AnimatedModels\\Alex\\AlexHand");

            foreach (ModelMesh mesh in sm.Model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    if (mesh.Name == "model.005")
                        effect.Texture = body;
                    if (mesh.Name == "model.006")
                        effect.Texture = head;
                    if (mesh.Name == "model.004")
                        effect.Texture = hand;
                    effect.EnableDefaultLighting();

                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 30;
                }
            }

            return sm;
        }
        static SkinnedModel GetAres(ContentManager content)
        {
            var sm = content.Load<SkinnedModel>("AnimatedModels\\Ares\\Model");
            var body = content.Load<Texture2D>("AnimatedModels\\Ares\\textura cor-2");
            var casque = content.Load<Texture2D>("AnimatedModels\\Ares\\UVcasque");

            foreach (ModelMesh mesh in sm.Model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    if (mesh.Name == "prometheu")
                        effect.Texture = body;

                    if (mesh.Name == "Hebras")
                        effect.Texture = casque;

                    effect.EnableDefaultLighting();

                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;
                }
            }
            return sm;
        }
        #endregion

        #region Characters
        private static void FillCharacters()
        {
            foreach (var kvp in AnimatedModels)
                Characters.Add(kvp.Key, new Character(null, kvp.Key, 0, null, new Vector3(0, 0, -17), new Vector3(1),
                                                      (kvp.Key == "Spiderman" || kvp.Key == "Alex") ? 1.6f : 1));
        }
        #endregion

        #region Themes
        static void LoadThemes(ContentManager content)
        {
            foreach (var dir in Directory.GetDirectories("Content\\Themes").Select(Path.GetFileNameWithoutExtension))
                Themes.Add(dir, CreateTheme(dir, content));
        }

        private static Dictionary<string, Texture2D> CreateTheme(string themeName, ContentManager content)
        {
            return new Dictionary<string, Texture2D> // je dois vérifier que le théme est complet...
                {
                    {"CheckBox.Selected", content.Load<Texture2D>("Themes\\" +themeName + "\\CheckBox\\Selected")},
                    {"CheckBox.Normal", content.Load<Texture2D>("Themes\\" +themeName + "\\CheckBox\\Normal")},

                    {"ListImageButtons.Top", content.Load<Texture2D>("Themes\\" +themeName + "\\ListImageButtons\\Top")},
                    {"ListImageButtons.Down", content.Load<Texture2D>("Themes\\" +themeName + "\\ListImageButtons\\Down")},
                    {"ListImageButtons.LeftSide", content.Load<Texture2D>("Themes\\" +themeName + "\\ListImageButtons\\LeftSide")},
                    {"ListImageButtons.RightSide", content.Load<Texture2D>("Themes\\" +themeName + "\\ListImageButtons\\RightSide")},
                    {"ListImageButtons.Background", content.Load<Texture2D>("Themes\\" +themeName + "\\ListImageButtons\\Background")},

                    {"ListLines.Top", content.Load<Texture2D>("Themes\\" +themeName + "\\ListLines\\Top")},
                    {"ListLines.Down", content.Load<Texture2D>("Themes\\" +themeName + "\\ListLines\\Down")},
                    {"ListLines.Separator", content.Load<Texture2D>("Themes\\" +themeName + "\\ListLines\\Separator")},
                    {"ListLines.LeftSide", content.Load<Texture2D>("Themes\\" +themeName + "\\ListLines\\LeftSide")},
                    {"ListLines.RightSide", content.Load<Texture2D>("Themes\\" +themeName + "\\ListLines\\RightSide")},
                    {"ListLines.Background", content.Load<Texture2D>("Themes\\" +themeName + "\\ListLines\\Background")},
                    {"ListLines.Scroll", content.Load<Texture2D>("Themes\\" +themeName + "\\ListLines\\Scroll")},

                    {"Slider.Top", content.Load<Texture2D>("Themes\\" +themeName + "\\Slider\\Top")},
                    {"Slider.Down", content.Load<Texture2D>("Themes\\" +themeName + "\\Slider\\Down")},
                    {"Slider.LeftSide", content.Load<Texture2D>("Themes\\" +themeName + "\\Slider\\LeftSide")},
                    {"Slider.RightSide", content.Load<Texture2D>("Themes\\" +themeName + "\\Slider\\RightSide")},
                    {"Slider.Cursor", content.Load<Texture2D>("Themes\\" +themeName + "\\Slider\\Cursor")},
                    {"Slider.BackgroundSelected", content.Load<Texture2D>("Themes\\" +themeName + "\\Slider\\BackgroundSelected")},
                    {"Slider.BackgroundNormal", content.Load<Texture2D>("Themes\\" +themeName + "\\Slider\\BackgroundNormal")},

                    {"Textbox.LeftSide", content.Load<Texture2D>("Themes\\" +themeName + "\\Textbox\\LeftSide")},
                    {"Textbox.RightSide", content.Load<Texture2D>("Themes\\" +themeName + "\\Textbox\\RightSide")},
                    {"Textbox.Separator", content.Load<Texture2D>("Themes\\" +themeName + "\\Textbox\\Separator")},
                    {"Textbox.Background", content.Load<Texture2D>("Themes\\" +themeName + "\\Textbox\\Background")}
                };
        }
        #endregion

        static void LoadInputKeys()
        {
            var keys = GameEngine.Config.Keys;
            var enumMovement = Enum.GetValues(typeof(Characters.Movement));
            for (int i = 0; i < 4; i++)
                if (keys.Length > i)
                {
                    var defaultKeys = keys[i].Split(',').ToList();
                    for (int j = 0; j < enumMovement.Length; j++)
                        InputKeys.Add(enumMovement.GetValue(j) + "." + i, new UniversalKeys(defaultKeys[j]));
                }
                else
                    for (int j = 0; j < enumMovement.Length; j++)
                        InputKeys.Add(enumMovement.GetValue(j) + "." + i, new UniversalKeys("Keyboard.Escape"));
        }

        public static void FillParticles(ContentManager content, Game game)
        {
            foreach (var mainDir in Directory.GetDirectories("Particles"))
                Particles.Add(Path.GetFileNameWithoutExtension(mainDir),
                              Directory.GetDirectories(mainDir)
                                       .ToDictionary(Path.GetFileNameWithoutExtension,
                                                     dir => new ParticleSystem(game, content, dir + '\\')));
        }
    }
}
