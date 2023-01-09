package tk.martilar.sprngtest;

import org.apache.tomcat.util.http.fileupload.IOUtils;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;
import org.springframework.web.servlet.ModelAndView;
import java.io.*;

@SpringBootApplication
@RestController
public class SprngtestApplication {
    public static String status = "Waiting...";

    public static void main(String[] args) {
        SpringApplication.run(SprngtestApplication.class, args);
    }

    @GetMapping("/")
    public ModelAndView Index() {
        ModelAndView modelAndView = new ModelAndView();
        modelAndView.setViewName("index.html");
        return modelAndView;
    }

    @PostMapping(value = "/api/upload", consumes = {"multipart/form-data"})
    public String uploadFile(@RequestParam("file") MultipartFile multipart) {
        System.out.println("Uploaded file Name : " + multipart.getOriginalFilename());
        status = multipart.getOriginalFilename();

        try {
            byte [] byteArr=multipart.getBytes();
            InputStream inputStream = new ByteArrayInputStream(byteArr);
            System.getProperty("java.io.tmpdir");

            File file = new File("javavideo.tmp");
            try(OutputStream outputStream = new FileOutputStream(file)) {
                IOUtils.copy(inputStream, outputStream);
            } catch (IOException e) {
                System.out.println("bozuk");
            }

            VideoProcessor.Process(file);
        } catch (Exception e) {
            throw new RuntimeException(e);
        }

        return multipart.getOriginalFilename();
    }

    @GetMapping("/api/status")
    public String Status() {
        return status;
    }
}
