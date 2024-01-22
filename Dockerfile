FROM public.ecr.aws/o2c0x5x8/application-base:dotnet-webapi-postgresql-entityframeworkcore

WORKDIR /usr/src/app/src

ADD src/ ./

WORKDIR /usr/src/app/tests

ADD tests/ ./

WORKDIR /usr/src/app

COPY build-and-run.sh ./

CMD ./build-and-run.sh
