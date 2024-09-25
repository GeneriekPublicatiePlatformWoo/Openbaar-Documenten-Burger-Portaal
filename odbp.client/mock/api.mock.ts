import { type MockHandler } from "vite-plugin-mock-server";

const mocks: MockHandler[] = [
  {
    pattern: "/api-mock/zoeken",
    method: "GET",
    handle: (_req, res) => {
      const data: number[] = [2, 3, 4, 5, 7];

      res.setHeader("Content-Type", "application/json");

      setTimeout(() => res.end(JSON.stringify(data)), 500);
    }
  }
];

export default mocks;
