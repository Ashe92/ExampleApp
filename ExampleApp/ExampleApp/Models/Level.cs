using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using ExampleApp.Enums;
using ExampleApp.Helpers;
using ExampleApp.Views;
using SkiaSharp;
using Xamarin.Forms.Internals;

namespace ExampleApp.Models
{
    public class Level
    {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        public int BlockWidth { get; private set; }
        public int BlockHeight { get; private set; }

        public int Number { get;  }
        public SKRect EndPoint => GetEndPoint();

        private List<Tile> TileElements { get; set; }

        private SKRect GetEndPoint()
        {
            var endPoint = new SKRect
            {
                Location = new SKPoint(100, 100),
                Size = new SKSize(100, 100)
            };
            return endPoint;
        }

        public Level(float width, float height, int number =0)
        {
            Number = number;
            CalculateWidth(width);
            CalculateHeight(height);
            TileElements = new List<Tile>();
        }

        public void CalculateMap()
        {
            var file =  ReadFile();
            SetUpMap(file);
        }

        public int[,] SetUpMap(string file)
        {
            var lines = file.Split(new[] { Environment.NewLine },StringSplitOptions.RemoveEmptyEntries);
            var map = new int[Constants.HeightElements, Constants.WidthElements];

            for (int i = 0; i < Constants.HeightElements; i++)
            {
                var splitted = lines[i].Split();
                for (int j = 0; j < Constants.WidthElements; j++)
                {
                    map[i, j] = Convert.ToInt32(splitted[j]);
                    SetupTile(i,j,map[i, j]);
                }
            }

            return map;
        }

        private void SetupTile(int i, int j, int tileType)
        {
            var tile = new Tile
            {
                Y = i * BlockHeight,
                X = j * BlockWidth,
                Size = new SKSize(BlockWidth, BlockHeight),
                Type = (TileType) tileType
            };
            TileElements.Add(tile);
        }


        private string ReadFile()
        {
            string text = "";
            try
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MapPage)).Assembly;
                Stream stream = assembly.GetManifestResourceStream($"ExampleApp.Resources.Level_{Number}.txt");

                using (var reader = new StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
            return text;
        }

        private void CalculateHeight(float canvasHeight)
        {
            var sizeOfBlock = (int)Math.Floor(canvasHeight / Constants.HeightElements);
            Height = (sizeOfBlock * Constants.HeightElements);
            BlockHeight = sizeOfBlock;

            var differece = canvasHeight - Height;
            StartX = (int) Math.Floor(differece / 2);
        }

        private void CalculateWidth(float canvasWidth)
        {
            var sizeOfBlock = (int)Math.Floor(canvasWidth / Constants.WidthElements); ;
            Width = sizeOfBlock * Constants.WidthElements;
            BlockWidth = sizeOfBlock;
            var differece = canvasWidth - Width;
            StartY = (int)Math.Floor(differece / 2);
        }
    }
}