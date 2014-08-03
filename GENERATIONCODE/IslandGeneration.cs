using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
//using System.Linq;
//using System.Text;

namespace IslandGame.GameWorld
{


    public class TestGenerator : IslandGenerator
    {




    public override  void generateIsland(ChunkSpace chunkSpace, SetPieceManager setPieceManager, JobSiteManager jobSiteManager, IslandLocationProfile locationProfile)
    {
     float magnitude = 2.0f;
     float frequency = .02f;
     float persistance = .25f;

     NoiseGenerator.setValuesForPass(1, 6, magnitude, frequency, persistance);
     NoiseGenerator.randomizeSeed();

     Random rand = new Random();

     float radius = ChunkSpace.chunkWidth * chunkSpace.widthInChunks / 2;
     for (int x = 0; x < ChunkSpace.chunkWidth * chunkSpace.widthInChunks; x++)
     {
        for (int z = 0; z < ChunkSpace.chunkWidth * chunkSpace.widthInChunks; z++)
        {

            float distFromCenter = (float)Math.Sqrt(Math.Pow(radius - x, 2) + Math.Pow(ChunkSpace.chunkWidth * chunkSpace.widthInChunks / 2 - z, 2));
                    float ratioFromCenter = (radius - distFromCenter) / (radius); //increases farther out
                    float centerCone = 1.0f-ratioFromCenter;
                    float heightNormal = (float)((float)Noise.Generate(x*.06f, z*.03f)+1f)/4.0f;
                    heightNormal += (float)((float)Noise.Generate(x*.02f+4, z*.02f+7)+1f)/4.0f;


                    heightNormal *= (float)Math.Pow(ratioFromCenter,.2);

                    /*if(heightNormal>.3){
                        heightNormal =1;
                    }
                    else{
                        heightNormal = 0;
                    }*/

                    heightNormal += (float)((float)NoiseGenerator.Noise(x, z) +1f) / 4.0f;

                    heightNormal = (float)Math.Pow((1.4f*heightNormal),91);

                    float heightNormalFirstPass = heightNormal;

                    if(heightNormal>.95){
                        heightNormal = .95f;
                    }

                    heightNormal -= .06f;




                    
                    //heightNormal -= .3f*(1f-ratioFromCenter);
                    
                    /*float smoothedCone =  1-((float)Math.Pow(centerCone,4f));
                    smoothedCone = (float)MathHelper.Clamp(smoothedCone,.1f,1)+.03f;
                    float smoothConePurturbation = (float)((float)NoiseGenerator.Noise(x+903, z+455) +.5f);
                    //smoothConePurturbation+=.5f;
                    heightNormal =  smoothConePurturbation ;
                    //heightNormal += 1;
                    //heightNormal *= 3;*/



                  /* heightNormal = 1f/heightNormal;

                   heightNormal +=1;
                    heightNormal = (float)Math.Pow(heightNormal,1.5f);
                    heightNormal -=1;

                    heightNormal *= smoothedCone;*/




                    


                    /*
                    if(heightNormal>=highBeachLimit){
                        heightNormal *= 4.0f;
                        heightNormal = (float)Math.Round(heightNormal);
                        heightNormal /= 4.0f;
                    }
                    else if (heightNormal>lowBeachLimit){
                        //heightNormal/=10.0f;
                        //highBeachLimit+=beachHeight;
                    }
                    */

                    

                  //  float erosion = ((float)NoiseGenerator.Noise(z*2 + 644, x*2 + 455) + .5f) *.5f * centerCone;

   

                  //  heightNormal *= 9;
                  //  heightNormal = (int)heightNormal;
                   // heightNormal/=10;

                   // erosion = ((float)Noise.Generate((float)z*.05f, (float)x*.05f) +1)/2.0f *.06f * (centerCone);
                  //  erosion = ((float)Noise.Generate((float)z*.03f, (float)x*.03f) +1)/2.0f *.05f * (centerCone);

                   // heightNormal = MathHelper.Clamp(heightNormal,0,1);

                    //heightNormal -= erosion/heightNormal;

                   // heightNormal -= .05f;


                   

                    //heightNormal should be done being set by now----------------------------------
                    int heightHere = (int)(heightNormal * ChunkSpace.chunkHeight);
                    for (int y = 0; y < ChunkSpace.chunkHeight; y++)
                    {
                        float yRatio = (float)y/(float)ChunkSpace.chunkHeight;

                        if( (heightNormalFirstPass>.3 && heightNormalFirstPass<.8) &&
                            Noise.Generate((float)x*.05f,(float)y*.05f,(float)z*.05f)<.0000001f){

                            continue;
                        }
                        else{

                        }

                        if (heightHere == y && y > 3)
                        {
                            if (rand.NextDouble() > .985)
                            {
                                //jobSiteManager.placeTree(
                                //new BlockLoc( locationProfile.profileSpaceToWorldSpace(
                                //        new IntVector3(x, y, z).toVector3())),
                                //         Tree.treeTypes.snowyPine);//
                            }
                        }
                        if (heightHere > y)
                        {

                                if(heightHere-1>y){

                                    float stoneWaving = (float)(Noise.Generate(x*.01f,z*.01f)+1)/2.0f;
                                    stoneWaving /= 18f;

                                    byte layerColor = 54;
                                    if(yRatio <.5+stoneWaving && yRatio > .3){
                                        layerColor = 53;
                                    }
                                    else if(yRatio <.7+stoneWaving && yRatio > .6+stoneWaving){
                                        layerColor = 51;
                                    }
                                    else if(yRatio <.73+stoneWaving && yRatio > .7+stoneWaving){
                                        layerColor = 50;
                                    }


                                    chunkSpace.setBlockAt( layerColor, x, y, z);
                                }
                                else{

                                    
                                        if (rand.NextDouble() > .999)
                                        {
                                            setPieceManager.placeDecorativePlant(new BlockLoc(locationProfile.profileSpaceToWorldSpace(new IntVector3(x, y, z).toVector3())));
                                        }

                                        chunkSpace.setBlockAt((y>2) ? (byte)57:(byte)56, x, y, z);
                                    

                               }

                           

                       }//
                       else if (y == 0)
                       {
                        chunkSpace.setBlockAt(PaintedCubeSpace.AIR, x, y, z);

                        }
                    
                }
                }

            }
        }
    }

}
