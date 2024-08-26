package com.ohgiraffers.webclient.config;

import lombok.RequiredArgsConstructor;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
public class TestController {

    private final WebClientUtil webClientUtil;

    private static final String REQUEST_URL = "http://192.168.1.45:7777/ai/edit_script";


    @GetMapping("/test-get")
    public String testGet() {
        return webClientUtil.getSync(REQUEST_URL, String.class);
    }

    @PostMapping("/test-post")
    public String testPost(@RequestBody Object requestBody) {
        return webClientUtil.postSync(REQUEST_URL, requestBody, String.class);
    }

    @PutMapping("/test-put")
    public String testPut(@RequestBody Object requestBody) {
        return webClientUtil.putSync(REQUEST_URL, requestBody, String.class);
    }

    @DeleteMapping("/test-delete")
    public String testDelete() {
        return webClientUtil.deleteSync(REQUEST_URL, String.class);
    }
}