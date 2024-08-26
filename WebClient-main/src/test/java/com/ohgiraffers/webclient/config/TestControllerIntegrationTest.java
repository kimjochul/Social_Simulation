package com.ohgiraffers.webclient.config;

import okhttp3.mockwebserver.MockResponse;
import okhttp3.mockwebserver.MockWebServer;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.web.client.TestRestTemplate;
import org.springframework.boot.test.web.server.LocalServerPort;
import org.springframework.http.*;

import static org.assertj.core.api.Assertions.assertThat;

@SpringBootTest(webEnvironment = SpringBootTest.WebEnvironment.RANDOM_PORT)
public class TestControllerIntegrationTest {

    @LocalServerPort
    private int port;

    @Autowired
    private TestRestTemplate restTemplate;

    private MockWebServer mockWebServer;

    @BeforeEach
    void setUp() throws Exception {
        mockWebServer = new MockWebServer();
        mockWebServer.start();
    }

    @AfterEach
    void tearDown() throws Exception {
        mockWebServer.shutdown();
    }

    @Test
    void testGetEndpoint() throws Exception {
        mockWebServer.enqueue(new MockResponse()
                .setBody("Mock response for GET")
                .addHeader("Content-Type", "application/json"));

        String url = "http://localhost:" + mockWebServer.getPort();
        ResponseEntity<String> response = restTemplate.getForEntity("/test-get?url=" + url, String.class);

        assertThat(response.getBody()).isEqualTo("Mock response for GET");
    }

    @Test
    void testPostEndpoint() throws Exception {
        mockWebServer.enqueue(new MockResponse()
                .setBody("Mock response for POST")
                .addHeader("Content-Type", "application/json"));

        String url = "http://localhost:" + mockWebServer.getPort();
        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(MediaType.APPLICATION_JSON);  // Content-Type을 JSON으로 설정
        HttpEntity<String> entity = new HttpEntity<>("{\"key\":\"value\"}", headers);

        ResponseEntity<String> response = restTemplate.exchange("/test-post?url=" + url, HttpMethod.POST, entity, String.class);

        assertThat(response.getBody()).isEqualTo("Mock response for POST");
    }

    // 추가로 PUT 및 DELETE 테스트도 비슷하게 작성 가능합니다.
}