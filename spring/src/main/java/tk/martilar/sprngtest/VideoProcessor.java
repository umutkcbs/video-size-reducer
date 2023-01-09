package tk.martilar.sprngtest;

import java.io.BufferedReader;
import java.io.File;
import java.io.InputStreamReader;
import java.util.Random;

public class VideoProcessor {
    public static void Process(File file) throws Exception {
        Random rand = new Random();
        String fileName = Integer.toString(rand.nextInt(1000000)) + ".mp4";

        ProcessBuilder builder = new ProcessBuilder(
                "cmd.exe", "/c", "ffmpeg -y -i javavideo.tmp -vf \"pad=ceil(iw/2)*2:ceil(ih/2)*2\" -acodec copy -threads 12 -vcodec libx264 -crf 28 " +  fileName);
        builder.redirectErrorStream(true);
        Process p = builder.start();
        SprngtestApplication.status = "Processing";
        BufferedReader r = new BufferedReader(new InputStreamReader(p.getInputStream()));
        String line;
        while (true) {
            line = r.readLine();
            SprngtestApplication.status = line;
            if (line == null) {
                SprngtestApplication.status = "Done!";
                Runtime.getRuntime().exec("explorer.exe /select," + fileName);
                File temp = new File("javavideo.tmp");
                temp.delete();
                break;
            }
            System.out.println(line);
        }
    }
}