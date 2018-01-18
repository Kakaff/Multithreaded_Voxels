# Multithreaded Voxels

![Alt Text](https://i.imgur.com/oS9aJRo.gif)

#### 11.468.800 Blocks (2800 Chunks) being generated and drawn in realtime.

Requires the player to be set to the experimental .net 4.6 version.

An experiment in creating a multithreaded minecraft esque "game" using Unity.

Attach the Debugger monobehaviour to a gameobject to use.
The default is 2 threads and the minimum requirement is 2 threads. In its current state it will not work with only 1 thread as the main thread is only used to pass data to and from threads into unity.

To change the threadcount simply change the "private int threads" in Debugger.cs to your desired value. Past 6 threads there doesn't however seem to be that much of an increase in performance. But i have tested it with up to 12 threads without any issues.

The threading relies on a workpool style system, where the mainthread will request work and put it in a workpool, the mainthread then tries to assign work to each thread and later poll each thread for completed work.

Distant chunks to get unloaded as the player moves around and new chunks get loaded in, but the full y axis is not yet generated.

To move around attach the Player monobehaviour to the maincamera, to add mouselook simply add the mouselook monobehaviour to the maincamera aswell. To activate/deactive mouselook and lock/unlock the cursor use left ctrl.


The project is currently on hold due to memory leaking issues which seem related to how unity's garbage collector works. (This could possibly be fixed however by reusing the subchunks similarly to entitypooling). It also lacks any form of compression so a decent amount of ram is recommended.

## Performance Comparision
Here are some gifs showing the speed of the world generation at different threadcounts.
The threadcount specified in the gif is the number of workerthreads.

All samples are run with 11.468.800 blocks (2800 chunks)

![Alt Text](https://i.imgur.com/ISlTHo7.gif)
![Alt Text](https://i.imgur.com/L31ypco.gif)
![Alt Text](https://i.imgur.com/wjSD6fw.gif)
![Alt Text](https://i.imgur.com/oS9aJRo.gif)
![Alt Text](https://i.imgur.com/M6YSGc1.gif)
