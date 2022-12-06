import http from 'k6/http';
import { randomString } from 'https://jslib.k6.io/k6-utils/1.2.0/index.js';
import { check, sleep } from 'k6';
export const options = {
    vus: 1,
    duration: '5m',
};

export default function () {
    const payload = JSON.stringify({
        Name: randomString(255)
    });
    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };
    const res1 = http.post('https://smv7pxjv3guwyaot6w4bftkju40jwfce.lambda-url.us-east-2.on.aws/', payload, params);
    check(res1, {
        'native is status 200': (r) => r.status === 200,
        'native duration was <= 500ms': (r) => r.timings.duration <= 500
    });
    const res2 = http.post('https://fjqfk6en5gg3aeaeoziivb5nhm0qddbw.lambda-url.us-east-2.on.aws/', payload, params);
    check(res2, {
        'runtime is status 200': (r) => r.status === 200,
        'runtime duration was <= 500ms': (r) => r.timings.duration <= 500
    });
    sleep(1);

}