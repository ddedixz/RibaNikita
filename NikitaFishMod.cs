using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Extensions;
using Nautilus.Handlers;
using UnityEngine;
using static CraftData;

namespace NikitaFishMod
{
    public class NikitaFishMod : ModBase
    {
        public static NikitaFishMod Instance { get; private set; }
        public static AssetBundle AssetBundle { get; private set; }
        
        public override void Start()
        {
            Instance = this;
            
            LoadAssetBundle();
            
            // Регистрируем рыбу
            RegisterNikitaFish();
            
            Console.WriteLine("[NikitaFishMod] Мод успешно загружен! Рыба Никита готова к вылову.");
        }

        private void LoadAssetBundle()
        {
            string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string bundlePath = Path.Combine(modFolder, "Assets", "nikitafishbundle");
            
            if (File.Exists(bundlePath))
            {
                AssetBundle = AssetBundle.LoadFromFile(bundlePath);
                Console.WriteLine("[NikitaFishMod] Ассеты загружены");
            }
        }

        private void RegisterNikitaFish()
        {
            // инфо о никите
            var fishInfo = PrefabInfo.WithTechType(
                "NikitaFish",                     // айдишник никиты
                "Рыба Никита",                     // никита на русском
                "Особая рыба с характером пидора. Смахивает на Чигура.",
                "en: Nikita Fish"                  // никита на пендосском
            );

  
            var fishPrefab = new CustomPrefab(fishInfo);

          
            fishPrefab.SetRecipe(new RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>()
                {
                    new Ingredient(TechType.RawFish, 2),  
                    new Ingredient(TechType.Salt, 1)
                }
            })
            .WithCraftingTime(2f);


            fishPrefab.SetGameObject(GetFishPrefab);
            fishPrefab.SetPdaGroupCategory(TechGroup.Fauna, TechCategory.Fauna);
            
    
            fishPrefab.SetSpawns(new SpawnLocation[] 
            { 
                new SpawnLocation(BiomeType.SafeShallows, 35f), 
                new SpawnLocation(BiomeType.KelpForest, 25f),
                new SpawnLocation(BiomeType.GrassyPlateaus, 20f)
            });


            fishPrefab.Register();
        }

        private IEnumerator GetFishPrefab(IOut<GameObject> gameObject)
        {
   
            var task = CraftData.GetPrefabForTechTypeAsync(TechType.Peeper);
            yield return task;
            
            var peeperPrefab = task.GetResult();
            var fishObject = GameObject.Instantiate(peeperPrefab);
            
         
            fishObject.name = "NikitaFish_Prefab";
            
  
            var fishName = fishObject.GetComponentInChildren<Creature>();
            if (fishName != null)
            {

                fishObject.AddComponent<NikitaFishBehaviour>();
            }
            
            // Меняем текстуру (если есть своя)
            /*if (AssetBundle != null)
            {
                var texture = AssetBundle.LoadAsset<Texture2D>("NikitaFishTexture");
                var renderer = fishObject.GetComponentInChildren<Renderer>();
                if (renderer != null && texture != null)
                {
                    renderer.material.mainTexture = texture;
                }
            }*/
            
            gameObject.Set(fishObject);
        }
    }
}