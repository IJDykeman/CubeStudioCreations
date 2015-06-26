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
                    float heightNormal = (float)((float)NoiseGenerator.Noise(x, z)+.5f)/2.0f;
                    
                    heightNormal += (float)((float)NoiseGenerator.Noise(x, z) +.5f) / 2.0f;


                    
                    //heightNormal -= .3f*(1f-ratioFromCenter);
                    
                    float smoothedCone =  1-((float)Math.Pow(centerCone,4f));
                    smoothedCone = (float)MathHelper.Clamp(smoothedCone,.1f,1)+.03f;
                    float smoothConePurturbation = (float)((float)NoiseGenerator.Noise(x+903, z+455) +.5f);
                    //smoothConePurturbation+=.5f;
                    heightNormal =  smoothConePurturbation ;
                    heightNormal += 1;
                    heightNormal *= 3;

                   // heightNormal *=3;
                  //  heightNormal =(float) Math.Pow(heightNormal,.1f);
                  //  heightNormal /=3;

                   heightNormal = 1f/heightNormal;

                   heightNormal +=1;
                    heightNormal = (float)Math.Pow(heightNormal,1.5f);
                    heightNormal -=1;

                    heightNormal *= smoothedCone;


                    float beachHeight = .02f;
                    float lowBeachLimit = .1f;
                    float highBeachLimit = .2f;//
                    if(heightNormal<highBeachLimit && heightNormal>lowBeachLimit){
                        heightNormal = beachHeight;
                    }
                    else  {
                        heightNormal -= highBeachLimit-beachHeight;
                        //heightNormal = (float)MathHelper.Clamp(heightNormal,.001f,1);
                    }

                    


                    
                    if(heightNormal>=highBeachLimit){
                        heightNormal *= 4.0f;
                        heightNormal = (float)Math.Round(heightNormal);
                        heightNormal /= 4.0f;
                    }
                    else if (heightNormal>lowBeachLimit){
                        //heightNormal/=10.0f;
                        //highBeachLimit+=beachHeight;
                    }


                    

                    float erosion = ((float)NoiseGenerator.Noise(z*2 + 644, x*2 + 455) + .5f) *.5f * centerCone;
                    //heightNormal -= erosion/(heightNormal+2.0f);
   

                    heightNormal *= 9;
                    heightNormal = (int)heightNormal;
                    heightNormal/=10;

                    erosion = ((float)Noise.Generate((float)z*.05f, (float)x*.05f) +1)/2.0f *.06f * (centerCone);
                    erosion = ((float)Noise.Generate((float)z*.03f, (float)x*.03f) +1)/2.0f *.05f * (centerCone);

                    heightNormal = MathHelper.Clamp(heightNormal,0,1);

                    heightNormal -= erosion/heightNormal;

                    heightNormal -= .05f;
                    //heightNormal = (float)Math.Pow(heightNormal*ChunkSpace.chunkHeight,.9f)/ChunkSpace.chunkHeight;
                    //heightNormal *=1.2f;

                    //heightNormal -= ((float)NoiseGenerator.Noise(z*4 + 454, x*4 + 4445) + .5f) *.2f ;
                    
                    heightNormal+=.1f;

                    //heightNormal should be done being set by now----------------------------------
                    int heightHere = (int)(heightNormal * ChunkSpace.chunkHeight);
                    for (int y = 0; y < ChunkSpace.chunkHeight; y++)
                    {
                        if( Noise.Generate((float)x*.05f,(float)y*.05f,(float)z*.05f)<.6f){
                            if (heightHere == y && y > 3)
                            {
                                if (rand.NextDouble() > .97 + .03 *(1.0f-heightNormal))
                                {
                                    if (rand.NextDouble() > .8f)
                                    //deciding which type of tree to place
                                    {
                                        jobSiteManager.placeTree(
                                         new BlockLoc(locationProfile.profileSpaceToWorldSpace(
                                                new IntVector3(x, y, z).toVector3())),
                                                 Tree.treeTypes.snowyLargePine);
                                    }
                                    else
                                    {
                                        jobSiteManager.placeTree(
                                         new BlockLoc(locationProfile.profileSpaceToWorldSpace(
                                                new IntVector3(x, y, z).toVector3())),
                                                 Tree.treeTypes.snowyPine);
                                    }
                                }
                            }
//
                        if (heightHere > y)
                        {
                            if(y<=1){
                                chunkSpace.setBlockAt( 37, x, y, z);//beach area
                            }
                            else{
                                if(heightHere-1>y){
                                    chunkSpace.setBlockAt( 34, x, y, z);
                                }
                                else{

                                    
                                        if (rand.NextDouble() > .999)
                                        {
                                            setPieceManager.placeDecorativePlant(new BlockLoc(locationProfile.profileSpaceToWorldSpace(new IntVector3(x, y, z).toVector3())));
                                        }

                                        chunkSpace.setBlockAt((y>2) ? (byte)15:(byte)14, x, y, z);
                                    

                               }

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

}
