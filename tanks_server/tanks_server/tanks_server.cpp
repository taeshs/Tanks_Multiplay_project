// CPPServer.cpp : 이 파일에는 'main' 함수가 포함됩니다. 거기서 프로그램 실행이 시작되고 종료됩니다.
//
#include <WinSock2.h>
#pragma comment(lib, "ws2_32.lib")
#include <iostream>
#include <thread>
#include <list>

using namespace std;

#define THREAD_COUNT (4)
#define PORT (8)

HANDLE g_iocp;

list<SOCKET> g_clientlist;
list<thread> g_threadlist;

typedef struct ClientSession {
    SOCKET sock;
    char buf[1024];
};

typedef struct ClientSession_ol {
    OVERLAPPED ol;
    WSABUF wsabuf;
};

void WorkerThread() {
    DWORD byteRecv = 0;
    DWORD flags = 0;
    ClientSession* cs;
    OVERLAPPED* ol;
    ClientSession_ol* cs_ol;
    bool result;

    while (1) {
        result = GetQueuedCompletionStatus(g_iocp, &byteRecv, (PULONG_PTR)&cs, (LPOVERLAPPED*)&ol, INFINITE);
        if (!result) {

        }
        cs_ol = (ClientSession_ol*)ol;
        /*
        cout<<cs_ol->wsabuf.buf<<endl;
        cout<<cs->buf<<endl;
        
        SENDALL;
        WSARECV;
        */
    }
}

int main()
{
    WSADATA wsa;
    WSAStartup(MAKEWORD(2, 2), &wsa);

    SOCKET lsock = WSASocket(AF_INET, SOCK_STREAM, IPPROTO_TCP, 0, 0, FILE_FLAG_OVERLAPPED);

    g_iocp = CreateIoCompletionPort(INVALID_HANDLE_VALUE, NULL, 0, 0); // iocp로 바꾸기

    SOCKADDR_IN saddr;
    saddr.sin_family = AF_INET;
    saddr.sin_addr.S_un.S_addr = ntohl(INADDR_ANY);
    saddr.sin_port = ntohs(8888);

    bind(lsock, (SOCKADDR*)&saddr, sizeof(saddr));

    listen(lsock, SOMAXCONN);

    SOCKET csock;
    SOCKADDR_IN caddr;
    int size_caddr = sizeof(caddr);

    for (int i = 0; i < THREAD_COUNT; i++) {
        g_threadlist.emplace_back(WorkerThread);
    }

    char buf[128];
    int byte;
    ClientSession* cs;
    ClientSession_ol* cs_ol;
    DWORD byteRecv = 0;
    DWORD flags = 0;

    while (1) {
        csock = accept(lsock, (SOCKADDR*)&caddr, &size_caddr);
        cout << "new client accepted." << endl;
        
        cs = new ClientSession;
        ZeroMemory(cs, sizeof(ClientSession));
        cs_ol = new ClientSession_ol;
        ZeroMemory(cs_ol, sizeof(ClientSession_ol));

        cs->sock = csock;
        cs_ol->wsabuf.buf = cs->buf;
        cs_ol->wsabuf.len = 1024;

        CreateIoCompletionPort((HANDLE)csock, g_iocp, (ULONG_PTR)&cs, 0);
        
        WSARecv(csock, &cs_ol->wsabuf, 1, &byteRecv, &flags, &cs_ol->ol, 0);
    }

    closesocket(csock);
    closesocket(lsock);
    WSACleanup();
}
