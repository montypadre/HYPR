using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace GameLogContract.Contracts.GameLog.ContractDefinition
{


    public partial class GameLogDeployment : GameLogDeploymentBase
    {
        public GameLogDeployment() : base(BYTECODE) { }
        public GameLogDeployment(string byteCode) : base(byteCode) { }
    }

    public class GameLogDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60c0604052600960808190526861737465726f69647360b81b60a090815261002a916002919061004f565b5034801561003757600080fd5b50600080546001600160a01b03191633179055610123565b82805461005b906100e8565b90600052602060002090601f01602090048101928261007d57600085556100c3565b82601f1061009657805160ff19168380011785556100c3565b828001600101855582156100c3579182015b828111156100c35782518255916020019190600101906100a8565b506100cf9291506100d3565b5090565b5b808211156100cf57600081556001016100d4565b600181811c908216806100fc57607f821691505b6020821081141561011d57634e487b7160e01b600052602260045260246000fd5b50919050565b6103a1806101326000396000f3fe608060405234801561001057600080fd5b50600436106100415760003560e01c806326e1c178146100465780635369e1b31461006d57806363ec7a8c14610080575b600080fd5b610059610054366004610209565b610095565b604051901515815260200160405180910390f35b61005961007b3660046102d5565b6100e8565b61009361008e3660046101e8565b61015d565b005b6001546000906001600160a01b031633146100cb5760405162461bcd60e51b81526004016100c2906102ed565b60405180910390fd5b506000818152600360208190526040909120015550600192915050565b6001546000906001600160a01b031633146101155760405162461bcd60e51b81526004016100c2906102ed565b60005b606481101561015657600083815260036020819052604090912001548314156101445750600192915050565b8061014e8161032e565b915050610118565b505b919050565b6000546001600160a01b031633146101af5760405162461bcd60e51b815260206004820152601560248201527414d95b99195c881b9bdd08185d5d1a1bdc9a5e9959605a1b60448201526064016100c2565b600180546001600160a01b0319166001600160a01b0392909216919091179055565b80356001600160a01b038116811461015857600080fd5b6000602082840312156101f9578081fd5b610202826101d1565b9392505050565b6000806000806080858703121561021e578283fd5b843567ffffffffffffffff80821115610235578485fd5b818701915087601f830112610248578485fd5b81358181111561025a5761025a610355565b604051601f8201601f19908116603f0116810190838211818310171561028257610282610355565b816040528281528a602084870101111561029a578788fd5b82602086016020830137876020848301015280985050505050506102c0602086016101d1565b93969395505050506040820135916060013590565b6000602082840312156102e6578081fd5b5035919050565b60208082526021908201527f596f7520617265206e6f7420617574686f72697a656420746f20646f207468696040820152607360f81b606082015260800190565b600060001982141561034e57634e487b7160e01b81526011600452602481fd5b5060010190565b634e487b7160e01b600052604160045260246000fdfea264697066735822122096e74639adac66a4f13ebf061a809ad67abeadf454f553f22d6e69af0e79ad5864736f6c63430008040033";
        public GameLogDeploymentBase() : base(BYTECODE) { }
        public GameLogDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class IsGameSeedAlreadyUsedFunction : IsGameSeedAlreadyUsedFunctionBase { }

    [Function("isGameSeedAlreadyUsed", "bool")]
    public class IsGameSeedAlreadyUsedFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_gameSeed", 1)]
        public virtual BigInteger GameSeed { get; set; }
    }

    public partial class UpdateGameInfoFunction : UpdateGameInfoFunctionBase { }

    [Function("updateGameInfo", "bool")]
    public class UpdateGameInfoFunctionBase : FunctionMessage
    {
        [Parameter("string", "_gameId", 1)]
        public virtual string GameId { get; set; }
        [Parameter("address", "_userAddress", 2)]
        public virtual string UserAddress { get; set; }
        [Parameter("uint256", "_score", 3)]
        public virtual BigInteger Score { get; set; }
        [Parameter("uint256", "_gameSeed", 4)]
        public virtual BigInteger GameSeed { get; set; }
    }

    public partial class UpdateGameServeAddressFunction : UpdateGameServeAddressFunctionBase { }

    [Function("updateGameServeAddress")]
    public class UpdateGameServeAddressFunctionBase : FunctionMessage
    {
        [Parameter("address", "_gameServeAddress", 1)]
        public virtual string GameServeAddress { get; set; }
    }

    public partial class IsGameSeedAlreadyUsedOutputDTO : IsGameSeedAlreadyUsedOutputDTOBase { }

    [FunctionOutput]
    public class IsGameSeedAlreadyUsedOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "seedUsed", 1)]
        public virtual bool SeedUsed { get; set; }
    }




}
