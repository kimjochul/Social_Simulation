package com.ohgiraffers.webclient.config;

import com.ohgiraffers.webclient.exception.InternalServerException;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatusCode;
import org.springframework.stereotype.Component;
import org.springframework.web.reactive.function.client.WebClient;
import org.springframework.http.HttpMethod;
import org.springframework.http.HttpStatus;

import reactor.core.publisher.Mono;

@Component
@RequiredArgsConstructor
public class WebClientUtil {

    private final WebClient webClient;

    // 동기 GET 요청
    public <T> T getSync(String url, Class<T> responseDtoClass) {
        return webClient.method(HttpMethod.GET)
                .uri(url)
                .retrieve()
                .onStatus(HttpStatusCode::is4xxClientError, clientResponse -> Mono.error(InternalServerException.EXCEPTION))
                .onStatus(HttpStatusCode::is5xxServerError, clientResponse -> Mono.error(InternalServerException.EXCEPTION))
                .bodyToMono(responseDtoClass)
                .block();
    }

    // 비동기 GET 요청
    public <T> Mono<T> getAsync(String url, Class<T> responseDtoClass) {
        return webClient.method(HttpMethod.GET)
                .uri(url)
                .retrieve()
                .onStatus(HttpStatusCode::is4xxClientError, clientResponse -> Mono.error(InternalServerException.EXCEPTION))
                .onStatus(HttpStatusCode::is5xxServerError, clientResponse -> Mono.error(InternalServerException.EXCEPTION))
                .bodyToMono(responseDtoClass);
    }

    // 동기 POST 요청
    public <T, V> T postSync(String url, V requestDto, Class<T> responseDtoClass) {
        return webClient.method(HttpMethod.POST)
                .uri(url)
                .bodyValue(requestDto)
                .retrieve()
                .onStatus(HttpStatusCode::is4xxClientError, clientResponse -> Mono.error(InternalServerException.EXCEPTION))
                .onStatus(HttpStatusCode::is5xxServerError, clientResponse -> Mono.error(InternalServerException.EXCEPTION))
                .bodyToMono(responseDtoClass)
                .block();
    }

    // 비동기 POST 요청
    public <T, V> Mono<T> postAsync(String url, V requestDto, Class<T> responseDtoClass) {
        return webClient.method(HttpMethod.POST)
                .uri(url)
                .bodyValue(requestDto)
                .retrieve()
                .onStatus(HttpStatusCode::is4xxClientError, clientResponse -> Mono.error(InternalServerException.EXCEPTION))
                .onStatus(HttpStatusCode::is5xxServerError, clientResponse -> Mono.error(InternalServerException.EXCEPTION))
                .bodyToMono(responseDtoClass);
    }

    // 동기 PUT 요청
    public <T, V> T putSync(String url, V requestDto, Class<T> responseDtoClass) {
        return webClient.method(HttpMethod.PUT)
                .uri(url)
                .bodyValue(requestDto)
                .retrieve()
                .onStatus(HttpStatusCode::is4xxClientError, clientResponse -> Mono.error(InternalServerException.EXCEPTION))
                .onStatus(HttpStatusCode::is5xxServerError, clientResponse -> Mono.error(InternalServerException.EXCEPTION))
                .bodyToMono(responseDtoClass)
                .block();
    }

    // 비동기 PUT 요청
    public <T, V> Mono<T> putAsync(String url, V requestDto, Class<T> responseDtoClass) {
        return webClient.method(HttpMethod.PUT)
                .uri(url)
                .bodyValue(requestDto)
                .retrieve()
                .onStatus(HttpStatusCode::is4xxClientError, clientResponse -> Mono.error(InternalServerException.EXCEPTION))
                .onStatus(HttpStatusCode::is5xxServerError, clientResponse -> Mono.error(InternalServerException.EXCEPTION))
                .bodyToMono(responseDtoClass);
    }

    // 동기 DELETE 요청
    public <T> T deleteSync(String url, Class<T> responseDtoClass) {
        return webClient.method(HttpMethod.DELETE)
                .uri(url)
                .retrieve()
                .onStatus(HttpStatusCode::is4xxClientError, clientResponse -> Mono.error(InternalServerException.EXCEPTION))
                .onStatus(HttpStatusCode::is5xxServerError, clientResponse -> Mono.error(InternalServerException.EXCEPTION))
                .bodyToMono(responseDtoClass)
                .block();
    }

    // 비동기 DELETE 요청
    public <T> Mono<T> deleteAsync(String url, Class<T> responseDtoClass) {
        return webClient.method(HttpMethod.DELETE)
                .uri(url)
                .retrieve()
                .onStatus(HttpStatusCode::is4xxClientError, clientResponse -> Mono.error(InternalServerException.EXCEPTION))
                .onStatus(HttpStatusCode::is5xxServerError, clientResponse -> Mono.error(InternalServerException.EXCEPTION))
                .bodyToMono(responseDtoClass);
    }
}
